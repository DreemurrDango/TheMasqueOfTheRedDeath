using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataCollection;

/// <summary>
/// 游戏管理器
/// </summary>
public class GameManager : Singleton<GameManager>
{
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

    [SerializeField]
    [Tooltip("本地玩家ID")]
    private PlayerID localPlayerID;

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
    [Tooltip("")]
    private List<Room> roomUIs;
}
