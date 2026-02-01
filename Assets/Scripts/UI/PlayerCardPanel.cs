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
    [SerializeField]
    [Tooltip("动画文本组件")]
    private AnimationText processAnimatedText;
    [SerializeField]
    [Tooltip("放置的卡牌动画起始位置RectTransform")]
    private RectTransform placedCardAnimStartPosRT;
    [SerializeField]
    [Tooltip("使用卡牌时遮罩显示图片")]
    private Image usedCardShowImage;
    [SerializeField]
    [Tooltip("使用卡牌时显示的卡牌详情面板")]
    private CardDetailShow usedCardShow;

    /// <summary>
    /// 当前玩家手牌UI列表
    /// </summary>
    private List<CardUI> inHandCardsList;

    /// <summary>
    /// 此面板所属的玩家ID
    /// </summary>
    public PlayerID BelongerID;

    public void InitClear()
    {
        playerID = PlayerID.COUNT;
        // 清空手牌实例
        foreach (var c in cardLayoutGroup.GetComponentsInChildren<CardUI>()) 
            Destroy(c.gameObject);
        inHandCardsList = new List<CardUI>();
        // 所使用卡牌显示隐藏
        usedCardShowImage.fillAmount = 0;
        // 等待加入文本动画
        processAnimatedText.strArgus[0] = "等待加入";
        processAnimatedText.enabled = true;
        cardNumText.text = "?";
    }
}
