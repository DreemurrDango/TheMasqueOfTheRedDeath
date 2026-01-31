using DataCollection;
using Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 卡牌在场景中的实例脚本
/// </summary>
public class CardUI : MonoBehaviour
{
    public enum CardState
    {
        InHand,
        Used,
        InRoom,
        InDoor
    }

    [Header("UI组件")]
    [SerializeField]
    [Tooltip("卡牌的描边")]
    private Image outlineImage;
    [SerializeField]
    [Tooltip("卡牌角色画像")]
    private Image portrait;
    [SerializeField]
    [Tooltip("身份能力说明文本")]
    private TMP_Text abilityDesText;
    [SerializeField]
    [Tooltip("所属阵营文本")]
    private TMP_Text affiliationText;
    [SerializeField]
    [Tooltip("卡牌总计数量文本")]
    private TMP_Text cardNumText;
    [SerializeField]
    [Tooltip("胜利条件文本")]
    private TMP_Text victoryConditionText;
    [SerializeField]
    [Tooltip("处于隐藏状态但对玩家可见时显示的游戏对线")]
    private CanvasGroup visableHideMask;
    [SerializeField]
    [Tooltip("显示卡牌名称的文本")]
    private TMP_Text cardNameText;
    [SerializeField]
    [Tooltip("卡牌反面朝上完全隐藏时显示的遮盖UI根对象")]
    private GameObject cardHideMaskGO;
    [SerializeField]
    [Tooltip("交互提示文本")]
    private TMP_Text interactTipText;
    [SerializeField]
    [Tooltip("显示卡牌最后来源的文本")]
    private TMP_Text sourceHistoryText;

    [Header("显示设置")]
    [SerializeField]
    [Tooltip("卡牌正常描边颜色")]
    private Color normalOutlineColor;
    [SerializeField]
    [Tooltip("卡牌选中描边颜色")]
    private Color hoverOutlineColor;
    [SerializeField]
    [Tooltip("卡牌UI的通常缩放大小")]
    private float normalScale = 1f;
    [SerializeField]
    [Tooltip("卡牌UI的悬停缩放大小")]
    private float hoverScale = 1.4f;
    [SerializeField]
    [Tooltip("卡牌缩放切换动画时间")]
    private float scaleTransitionTime = 0.3f;

    /// <summary>
    /// 当前所有者或最后的打出者的ID
    /// </summary>
    private PlayerID belongerID;
    /// <summary>
    /// 当前卡牌所处位置状态
    /// </summary>
    private CardState cardState;
    /// <summary>
    /// 此卡牌的信息
    /// </summary>
    private CardInfo cardInfo;
}
