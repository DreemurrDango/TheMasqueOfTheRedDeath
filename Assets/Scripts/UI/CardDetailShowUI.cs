using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using DataCollection;
using Enums;

public class CardDetailShow : MonoBehaviour
{
    [Header("UI组件")]
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

    public void ShowCardDetail(Sprite portraitSprite, string abilityDes, string affiliation, int cardNum, string victoryCondition, string cardName, bool isVisableHide, bool isHide)
    {
        portrait.sprite = portraitSprite;
        abilityDesText.text = abilityDes;
        affiliationText.text = affiliation;
        cardNumText.text = $"数量：{cardNum}";
        victoryConditionText.text = victoryCondition;
        cardNameText.text = cardName;
        //visableHideMask.alpha = isVisableHide ? 1f : 0f;
        visableHideMask.gameObject.SetActive(isVisableHide);
        cardHideMaskGO.SetActive(isHide);
    }

    /// <summary>
    /// 显示卡牌详情
    /// </summary>
    /// <param name="id">卡牌ID</param>
    /// <param name="isVisableHide">是否可见隐藏</param>
    /// <param name="isHide">是否隐藏</param>
    public void ShowCardDetail(CardID id,bool isVisableHide,bool isHide)
    {
        var info = GameManager.Instance.GetCardInfo(id);
        var num = GameManager.Instance.GetCardNum(id);
        ShowCardDetail(info.portrait, info.abilityDesc, info.AffiliationName, num, info.victoryCondition, info.CardName, isVisableHide, isHide);
    }
}
