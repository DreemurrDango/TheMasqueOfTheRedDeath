using DataCollection;
using Enums;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

/// <summary>
/// 卡牌在场景中的实例脚本
/// </summary>
public class CardUI : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
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
    /// 已查看此卡牌后，拥有查看权力的玩家ID列表
    /// </summary>
    private List<PlayerID> visablePlayer;
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
    private CardID cardID;
    /// <summary>
    /// 是否处于隐藏，非公开状态
    /// </summary>
    private bool _isHidden = false;

    /// <summary>
    /// 获取其中的卡牌信息
    /// </summary>
    public CardID ID => cardID;
    /// <summary>
    /// 是否盖卡处于隐藏非公开状态
    /// </summary>
    public bool IsHidden
    {
        get => _isHidden;
        set
        {
            _isHidden = value;
            UpdateShow();
        }
    }

    /// <summary>
    /// 初始化卡牌显示
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cardState"></param>
    public void Init(CardID id,CardState cardState,bool isHidden = false,PlayerID belonger = PlayerID.COUNT)
    {
        // 初始化基本数据
        cardID = id;
        visablePlayer = new List<PlayerID>();
        if(belonger != PlayerID.COUNT) visablePlayer.Add(belonger);
        _isHidden = isHidden;
        this.belongerID = belonger;
        this.cardState = cardState;
        var info = GameManager.Instance.GetCardInfo(cardID);
        var num = GameManager.Instance.GetCardNum(cardID);
        // UI显示内容
        cardNameText.text = info.CardName;
        cardNameText.color = info.nameColor;
        portrait.sprite = info.portrait;
        portrait.color = info.portColor;
        abilityDesText.text = info.abilityDesc;
        affiliationText.text = info.AffiliationName;
        affiliationText.color = info.afiliationTextColor;
        cardNumText.text = num.ToString();
        victoryConditionText.text = $"<b>胜利条件</b>:{info.victoryCondition}";
        // 初始隐藏相关UI
        UpdateShow();
        interactTipText.gameObject.SetActive(false);
        //sourceHistoryText.gameObject.SetActive(false);
    }

    /// <summary>
    /// 设置此卡牌实例对玩家的可见性
    /// </summary>
    /// <param name="playerID">要设置的玩家ID</param>
    /// <param name="visable">是否可见</param>
    public void SetVisable(PlayerID playerID, bool visable)
    {
        if (visable)
        {
            if (!visablePlayer.Contains(playerID))
                visablePlayer.Add(playerID);
        }
        else
        {
            if (visablePlayer.Contains(playerID))
                visablePlayer.Remove(playerID);
        }
        UpdateShow();
    }

    /// <summary>
    /// 显示互动提示文本内容
    /// </summary>
    /// <param name="tipText">互动提示文本内容，设为空文本时关闭显示</param>
    public void ShowInteractTip(string tipText)
    {
        interactTipText.text = tipText;
        interactTipText.gameObject.SetActive(string.IsNullOrEmpty(tipText));
    }

    /// <summary>
    /// 更新显示状态
    /// </summary>
    private void UpdateShow()
    {
        // 设置为隐藏状态
        if (IsHidden)
        {
            var visable = visablePlayer != null && visablePlayer.Contains(GameManager.Instance.LocalPlayerID);
            cardHideMaskGO.SetActive(!visable);
            visableHideMask.alpha = visable ? 1f : 0f;
        }
        // 设置为公开状态
        else
        {
            cardHideMaskGO.SetActive(false);
            visableHideMask.alpha = 0f;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
