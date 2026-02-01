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


    [SerializeField, ReadOnly]
    [Tooltip("本地玩家ID")]
    private PlayerID localPlayerID = PlayerID.COUNT;
    /// <summary>
    /// 玩家的IP地址与玩家ID映射字典
    /// </summary>
    private Dictionary<IPEndPoint,PlayerID> playerIPIDdict;
    
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
    /// 获取本地玩家ID
    /// </summary>
    public PlayerID LocalPlayerID => localPlayerID;

    private void OnEnable()
    {
        tcpServer.OnClientConnected += OnClientConnected;
        tcpServer.OnClientDisconnected += OnClientDisconnected;
        tcpServer.OnReceivedMessage += OnReceiveC2SMessage;
        tcpClient.OnReceivedMessage += OnReceiveS2CMessage;
    }

    private void OnReceiveS2CMessage(string message)
    {
        var baseMsg = JsonConvert.DeserializeObject<MessageDataBase>(message);
        switch (baseMsg.messageType)
        {
            case MessageType.S2C_RoomPlayerUpdated:
                var rpu = JsonUtility.FromJson<S2C_RoomPlayerUpdated>(message);
                if (!IsServer) playerIPIDdict = rpu.playerIPIDDict;
                // UI更新
                
                break;
        }
    }

    private void OnReceiveC2SMessage(IPEndPoint point, string message)
    {
        throw new NotImplementedException();
    }

    private void OnClientDisconnected(IPEndPoint ipep)
    {
        playerIPIDdict.Remove(ipep);
    }

    private void OnClientConnected(IPEndPoint ipep)
    {
        playerIPIDdict.Add(ipep, (PlayerID)playerIPIDdict.Count);
    }

    private void Start()
    {
        JsonHelper.LoadFromStreamingAssets<GameConfig>(GameConfigFilePath,out var gameConfig);
        // 初始化数据
        playerIPIDdict = new();
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

    private void SendS2CMessage_RoomPlayerUpdated(Dictionary<IPEndPoint,PlayerID> playerIPIDDict)
    {
        var msg = new S2C_RoomPlayerUpdated
        {
            messageType = MessageType.S2C_RoomPlayerUpdated,
            playerIPIDDict = playerIPIDDict
        };
        var json = msg.ToJson();
        tcpServer.BroadcastMessage(json);
    }
}
