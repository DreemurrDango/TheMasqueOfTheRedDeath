using Enums;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using DataCollection;

/// <summary>
/// 玩家卡牌面板，控制各个玩家手牌的显示与交互，同时存储玩家手牌数据
/// </summary>
public class PlayerCardPanel : MonoBehaviour
{
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

    [SerializeField, ReadOnly]
    [Tooltip("所属玩家ID")]
    private PlayerID playerID;
    /// <summary>
    /// 当前玩家手牌UI列表
    /// </summary>
    private List<CardUI> inHandCardsList;

    /// <summary>
    /// 此面板所属的玩家ID
    /// </summary>
    public PlayerID BelongerID => playerID;
    /// <summary>
    /// 当前手牌数量
    /// </summary>
    public int HandCardCount => inHandCardsList.Count;
    /// <summary>
    /// 获取是否属于本地玩家
    /// </summary>
    public bool BelongToLocal => playerID == GameManager.Instance.LocalPlayerID;

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
        processAnimatedText.SetText("等待加入", true);
        cardNumText.text = "?";
    }

    public void InitBelonger(PlayerInfo playerInfo)
    {
        this.playerID = playerInfo.id;
        playerNameText.text = playerInfo.name;
        playerNameText.color = playerInfo.nameColor;
        processAnimatedText.SetText("已加入", false);
    }

    /// <summary>
    /// 游玩正式开始时初始化
    /// </summary>
    /// <param name="info">所对应的玩家信息</param>
    /// <param name="startHandCards">起始手牌信息</param>
    /// <param name="animInterval">动画间隔</param>
    /// <param name="completedWaitTime">结束后的等待时间</param>
    /// <param name="onCompletedAction">结束后的动作</param>
    public void InitGameplayStart(PlayerInfo info,List<CardID> startHandCards,float animInterval,float completedWaitTime,Action<PlayerID> onCompletedAction)
    {
        // 设置玩家信息
        this.playerID = info.id;
        playerNameText.text = info.name;
        playerNameText.color = info.nameColor;
        var localPlayer = GameManager.Instance.LocalPlayerID;
        var belongToLocal = playerID == localPlayer;
        processAnimatedText.SetText("分发手牌中", true);
        if (startHandCards == null) startHandCards = new List<CardID>();
        // 播放卡牌入库动画
        var sequence = DOTween.Sequence();
        for (int i = 0; i < startHandCards.Count; i++)
        {
            CardUI card;
            sequence.AppendCallback(() => 
            {
                card = AddCard();
                card.Init(startHandCards[i], CardState.InHand, !belongToLocal, playerID);
            });
            sequence.AppendInterval(animInterval);
        }
        sequence.onComplete += () => onCompletedAction(playerID);
    }

    /// <summary>
    /// 生成卡牌UI实例，并将其添加到手牌区域中
    /// </summary>
    private CardUI AddCard()
    {
        var cardGO = Instantiate(cardUIPrefab.gameObject, cardLayoutGroup.transform);
        var card = cardGO.GetComponent<CardUI>();
        (cardGO.transform as RectTransform).pivot = new Vector2(0.5f, 0f);
        inHandCardsList.Add(card);
        LayoutRebuilder.MarkLayoutForRebuild(cardLayoutGroup.transform as RectTransform);
        return card;
    }

    /// <summary>
    /// 显示回合开始信息
    /// </summary>
    /// <param name="turnPlayerID"></param>
    /// <param name="turn"></param>
    public void ShowTurnStartInfo(PlayerID turnPlayerID,int turn)
    {
        if(turnPlayerID == playerID && playerID == GameManager.Instance.LocalPlayerID)
        {
            // 本地玩家回合开始时动作
            processAnimatedText.SetText("你的回合！",false);
            // TODO:当前玩家可行动时逻辑
        }
        else if(turnPlayerID == playerID)
        {
            processAnimatedText.SetText("思考中", true);
        }
        else processAnimatedText.SetText("", false);
    }
}
