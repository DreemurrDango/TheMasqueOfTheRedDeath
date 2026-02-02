using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataCollection;
using Sirenix.OdinInspector;
using DreemurrStudio.Network;
using Utilities;
using System.Net;
using System;
using Newtonsoft.Json;
using System.Linq;
using DreemurrStudio.CollectionExtension;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : Singleton<GameManager>
{
    public const string GameConfigFilePath = "gameConfig.json";

    /// <summary>
    /// 获取玩家的数量
    /// </summary>
    public static int PlayerNum => ((int)PlayerID.COUNT);
    /// <summary>
    /// 当前游戏流程状态
    /// </summary>
    public enum GameState
    {
        RoomWaitting,
        Gameplay,
        GameResult
    }

    [System.Serializable]
    /// <summary>
    /// 卡牌梳理配置
    /// </summary>
    public struct CardNumConfig
    {
        /// <summary>
        /// 卡牌ID
        /// </summary>
        public CardID cardID;
        /// <summary>
        /// 每轮游玩中的数量
        /// </summary>
        public int CardNum;
    }

    [System.Serializable]
    public struct GameConfig
    {
        public string ipAddress;
        public string serverIPAddress;
        public bool isServer;
    }

    [SerializeField]
    [Tooltip("服务器端口号")]
    private int serverPort = 8888;
    [SerializeField]
    [Tooltip("TCP客户端")]
    private TCPClient tcpClient;
    [SerializeField]
    [Tooltip("TCP服务器端")]
    private TCPServer tcpServer;
    [SerializeField]
    [Tooltip("卡牌信息配置SO")]
    private CardInfoConfig_SO cardInfoSO;

    [Header("游玩配置")]
    [SerializeField]
    [Tooltip("玩家配置列表")]
    private List<PlayerInfo> playerInfos;
    [SerializeField]
    [Tooltip("每次游玩的卡牌数量配置列表")]
    private List<CardNumConfig> gameCardNumConfigList;
    [SerializeField]
    [Tooltip("每名玩家起始手牌数量")]
    private int startHandCardNum = 5;
    [SerializeField]
    [Tooltip("起始时默认盖卡数量")]
    private int startHideCardNum = 4;

    [Header("UI组件")]
    [SerializeField]
    [Tooltip("房间UI列表")]
    private List<Room> roomUIs;
    [SerializeField]
    [Tooltip("门UI列表")]
    private List<DoorUI> doors;
    [SerializeField]
    [Tooltip("玩家手牌面板列表，第一个为本地玩家的手牌面板")]
    private List<PlayerCardPanel> playerCardPanels;

    [SerializeField,ReadOnly]
    /// <summary>
    /// 当前游戏流程
    /// </summary>
    private GameState gameState;
    /// <summary>
    /// 玩家的IP地址与玩家ID映射字典
    /// </summary>
    private Dictionary<IPEndPoint,PlayerID> playerIPIDdict;
    /// <summary>
    /// 当前游戏轮数
    /// </summary>
    private int gameTurn;
    /// <summary>
    /// 当前轮的行动玩家序号
    /// </summary>
    private int turnPlayerIndex;
    /// <summary>
    /// 每一轮的起始玩家ID
    /// </summary>
    private PlayerID startPlayer;
    
    /// <summary>
    /// 当前是否作为服务器进行游戏
    /// </summary>
    public bool IsServer => tcpServer.InRunning;
    /// <summary>
    /// 根据卡牌ID获取卡牌信息
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public CardInfo GetCardInfo(CardID id) => cardInfoSO.GetInfo(id);
    /// <summary>
    /// 获取卡牌在卡组中的总张数
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetCardNum(CardID id) => gameCardNumConfigList.Find(c => c.cardID == id).CardNum;
    /// <summary>
    /// 获取目标回合玩家
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public PlayerID GetTurnPlayer(int indexInTurn) => (PlayerID)((((int)startPlayer) + indexInTurn) % PlayerNum);
    /// <summary>
    /// 获取玩家的手牌区域界面
    /// </summary>
    /// <param name="id">玩家ID</param>
    /// <returns></returns>
    public PlayerCardPanel GetPlayerCardPanel(PlayerID id) => playerCardPanels[((int)id)];
    /// <summary>
    /// 获取玩家配置信息
    /// </summary>
    /// <param name="id">玩家ID</param>
    /// <returns></returns>
    public PlayerInfo GetPlayerInfo(PlayerID id) => playerInfos[((int)id)];
    /// <summary>
    /// 获取本地玩家的IP地址
    /// </summary>
    /// <returns></returns>
    public IPEndPoint LocalIPEP => tcpClient.ClientIPEP;
    /// <summary>
    /// 获取本地玩家ID
    /// </summary>
    public PlayerID LocalPlayerID => playerIPIDdict[LocalIPEP];
    

    private void OnEnable()
    {
        tcpServer.OnClientConnected += OnClientConnected;
        tcpServer.OnClientDisconnected += OnClientDisconnected;
        tcpServer.OnReceivedMessage += OnReceiveC2SMessage;
        tcpClient.OnReceivedMessage += OnReceiveS2CMessage;
    }

    private void Start()
    {
        JsonHelper.LoadFromStreamingAssets<GameConfig>(GameConfigFilePath,out var gameConfig);
        // 初始化数据
        playerIPIDdict = new();
        gameState = GameState.RoomWaitting;
        // 初始化UI状态
        playerCardPanels.ForEach(p => p.InitClear());
        roomUIs.ForEach(r => r.InitClear());
        doors.ForEach(d => d.InitClear());
        // 建立TCP连接
        if (gameConfig.isServer)
        {
            // 主持服务器并将客户端连接到服务器
            tcpServer.StartServer(gameConfig.ipAddress,serverPort,false);
            tcpClient.ConnectToServer(gameConfig.ipAddress,0,gameConfig.serverIPAddress,serverPort);
        }
        else tcpClient.ConnectToServer(gameConfig.ipAddress, 0, gameConfig.serverIPAddress, serverPort);
    }

    /// <summary>
    /// 客户端处理接收到的来自服务器的信息
    /// 由客户端处理后进行相关UI的显示和数据更新
    /// </summary>
    /// <param name="message"></param>
    private void OnReceiveS2CMessage(string message)
    {
        var baseMsg = JsonConvert.DeserializeObject<MessageDataBase>(message);
        switch (baseMsg.messageType)
        {
            case MessageType.S2C_RoomPlayerUpdated:
                if (gameState != GameState.RoomWaitting) return;
                var rpu = JsonUtility.FromJson<S2C_RoomPlayerUpdated>(message);
                if (!IsServer) playerIPIDdict = rpu.playerIPIDDict;
                for (int i = 0;i < playerIPIDdict.Count; i++)
                {
                    var aimID = (PlayerID)((((int)LocalPlayerID) + i) % PlayerNum);
                    playerCardPanels[((int)aimID)].InitBelonger(GetPlayerInfo(aimID));
                }
                break;
            case MessageType.S2C_GameplayStarted:
                if(gameState != GameState.RoomWaitting) return;
                var gs = JsonUtility.FromJson<S2C_GameplayStarted>(message);
                var dict = gs.playerStartHandCardsDict;
                gameState = GameState.Gameplay;
                // 更新UI，监听动画流程结束回调
                for(int i = 0; i < PlayerNum; i++)
                {
                    var aimID = (PlayerID)((((int)LocalPlayerID) + i) % PlayerNum);
                    playerCardPanels[((int)aimID)].InitGameplayStart(GetPlayerInfo(aimID),dict[aimID],0.5f,1f,(id) =>
                    {
                        if (!IsServer || id != LocalPlayerID) return;
                        SendS2CMessage_PlayerTurnStarted(1, 0);
                    });
                }
                break;
            case MessageType.S2C_PlayerTurnStarted:
                if (gameState != GameState.Gameplay) return;
                var pts = JsonUtility.FromJson<S2C_PlayerTurnStarted>(message);
                gameTurn = pts.turn;
                turnPlayerIndex = pts.turnPlayerIndex;
                foreach (var p in playerCardPanels) 
                    p.ShowTurnStartInfo(pts.turnPlayerID, gameTurn);
                break;
        }
    }

    private void OnReceiveC2SMessage(IPEndPoint ipep, string message)
    {
    }

    private void OnClientDisconnected(IPEndPoint ipep)
    {
        playerIPIDdict.Remove(ipep);
        SendS2CMessage_RoomPlayerUpdated(playerIPIDdict);
    }


    /// <summary>
    /// 服务器处理客户端连接时动作
    /// 发送房间玩家信息更新事件，人数凑齐时正式开始游戏流程
    /// </summary>
    /// <param name="ipep"></param>
    private void OnClientConnected(IPEndPoint ipep)
    {
        if(gameState != GameState.RoomWaitting) return;
        var keyList = playerIPIDdict.Keys.ToList();
        for (int i = 0; i < playerIPIDdict.Count; i++)
            playerIPIDdict[keyList[i]] = (PlayerID)i;
        playerIPIDdict.Add(ipep, (PlayerID)playerIPIDdict.Count);
        SendS2CMessage_RoomPlayerUpdated(playerIPIDdict);
        // 玩家人数凑齐时，正式开始游戏，先随机每名玩家的起始手牌
        if (playerIPIDdict.Count == PlayerNum)
        {
            // -- 开始新游戏轮的初始化
            gameTurn = 0;
            turnPlayerIndex = 0;
            startPlayer = PlayerID.COUNT;
            gameState = GameState.Gameplay;
            // -- 生成乱序总牌堆
            List<CardID> cardList = new();
            foreach (var cardNumConfig in gameCardNumConfigList)
            {
                for (int i = 0; i < cardNumConfig.CardNum; i++)
                    cardList.Add(cardNumConfig.cardID);
            }
            cardList.Shuffle();
            // -- 分发起始手牌
            Dictionary<PlayerID,List<CardID>> playerStartHandCards = new();
            for (int i = 0; i < playerIPIDdict.Count; i++)
            {
                var aimID = (PlayerID)i;
                var handCards = cardList.GetRange(i * startHandCardNum, (i + 1) * startHandCardNum - 1);
                playerStartHandCards.Add(aimID, handCards);
                // 拥有唯一一张亲王的玩家标记为起始玩家
                if (startPlayer == PlayerID.COUNT && handCards.Contains(CardID.亲王))
                    startPlayer = aimID;
            }
            // --发送游戏开始消息
            SendS2CMessage_GameplayStart(playerStartHandCards);
        }
    }

    #region 服务器端方法
    /// <summary>
    /// 获取基于当前玩家ID的前一个玩家ID
    /// </summary>
    /// <param name="currentPlayerID">当前玩家ID</param>
    /// <param name="activeRequired">是否要求为未锁定的玩家</param>
    /// <returns></returns>
    public PlayerID? GetPrevPlayerID(PlayerID currentPlayerID, bool activeRequired)
    {
        for (int i = 1; i < PlayerNum; i++)
        {
            var index = (((int)currentPlayerID) + PlayerNum - i) % PlayerNum;
            if(!activeRequired || playerCardPanels[index].HandCardCount > 1)
                return (PlayerID)index;
        }
        return null;
    }

    /// <summary>
    /// 获取基于当前玩家ID的下一个玩家ID
    /// </summary>
    /// <param name="currentPlayerID"></param>
    /// <param name="activeRequired"></param>
    /// <returns></returns>
    public PlayerID? GetNextPlayerID(PlayerID currentPlayerID, bool activeRequired)
    {
        for (int i = 1; i < PlayerNum; i++)
        {
            var index = (((int)currentPlayerID) + i) % PlayerNum;
            if (!activeRequired || playerCardPanels[index].HandCardCount > 1)
                return (PlayerID)index;
        }
        return null;
    }
    #endregion

    #region 配置读写
    [SerializeField]
    [Tooltip("游戏配置")]
    private GameConfig gameConfig;
    [Button("保存配置")]
    public void SaveGameConfig() => JsonHelper.SaveToStreamingAssets(gameConfig, GameConfigFilePath, false);
    #endregion

    #region 服务器到客户端 TCP消息
    /// <summary>
    /// 向所有客户端发送房间玩家更新消息
    /// </summary>
    /// <param name="playerIPIDDict">更新后的玩家IP对照ID映射字典</param>
    private void SendS2CMessage_RoomPlayerUpdated(Dictionary<IPEndPoint,PlayerID> playerIPIDDict)
    {
        var msg = new S2C_RoomPlayerUpdated
        {
            messageType = MessageType.S2C_RoomPlayerUpdated,
            playerIPIDDict = playerIPIDDict
        };
        var json = msg.ToJson();
        tcpServer.SendToAllClients(json);
    }

    /// <summary>
    /// 向所有客户端发送游戏开始消息
    /// </summary>
    /// <param name="startHandCards">开始时的手牌信息字典</param>
    private void SendS2CMessage_GameplayStart(Dictionary<PlayerID,List<CardID>> startHandCards)
    {
        var msg = new S2C_GameplayStarted
        {
            messageType = MessageType.S2C_GameplayStarted,
            playerStartHandCardsDict = startHandCards
        };
        var json = msg.ToJson();
        tcpServer.SendToAllClients(json);
    }

    /// <summary>
    /// 向所有客户端发送玩家回合开始消息
    /// </summary>
    /// <param name="turn">回合数</param>
    /// <param name="turnPlayerIndex">要开始回合的玩家在回合中的运行序号</param>
    private void SendS2CMessage_PlayerTurnStarted(int turn,int turnPlayerIndex)
    {
        var msg = new S2C_PlayerTurnStarted
        {
            messageType = MessageType.S2C_PlayerTurnStarted,
            turnPlayerID = (PlayerID)((((int)startPlayer) + turnPlayerIndex)%PlayerNum),
            turnPlayerIndex = turnPlayerIndex
        };
        var json = msg.ToJson();
        tcpServer.SendToAllClients(json);
    }
    #endregion
}
