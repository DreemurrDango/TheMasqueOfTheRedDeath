using Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardPanel : MonoBehaviour
{
    [SerializeField]
    [Tooltip("所属玩家ID")]
    private PlayerID playerID;
    [SerializeField]
    [Tooltip("根画布组")]
    private CanvasGroup canvasGroup;
    [SerializeField]
    [Tooltip("卡牌的根布局组")]
    private LayoutGroup cardLayoutGroup;
    [SerializeField]
    [Tooltip("卡牌UI预制体")]
    private CardUI cardUIPrefab;
    [SerializeField]
    [Tooltip("显示玩家名称的文本")]
    private TMP_Text playerNameText;
    [SerializeField]
    [Tooltip("手牌剩余数量显示文本")]
    private TMP_Text cardNumText;
    [SerializeField]
    [Tooltip("游戏进程提示文本")]
    private TMP_Text gameProcessText;

    /// <summary>
    /// 当前玩家手牌UI列表
    /// </summary>
    private List<CardUI> inHandCardsList;
}
