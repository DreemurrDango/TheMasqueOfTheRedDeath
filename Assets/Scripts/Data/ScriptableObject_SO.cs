using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataCollection;
using DreemurrStudio.CollectionExtension;
using Enums;

/// <summary>
/// 数据库模板类
/// </summary>
[CreateAssetMenu(fileName = "CardInfoConfig", menuName = "数据/卡牌信息配置表")]
public class CardInfoConfig_SO : ScriptableObject
{
    [Header("卡牌信息列表")]
    [SerializeField]
    [Tooltip("所有卡牌的信息列表")]
    private List<CardInfo> cardInfoList;

    /// <summary>
    /// 卡牌ID对应的卡牌信息字典
    /// </summary>
    private Dictionary<CardID,CardInfo> _cardInfoDic;
    /// <summary>
    /// 卡牌ID对应的卡牌信息字典
    /// </summary>
    public Dictionary<CardID, CardInfo> CardInfoDic
    {
        get
        {
            if (_cardInfoDic == null)
            {
                _cardInfoDic = new Dictionary<CardID, CardInfo>();
                foreach (var cardInfo in cardInfoList) 
                    _cardInfoDic[cardInfo.id] = cardInfo;
            }
            return _cardInfoDic;
        }
    }
    /// <summary>
    /// 获取所有卡牌信息
    /// </summary>
    /// <returns>卡牌信息列表</returns>
    public List<CardInfo> GetAllCardInfo() => cardInfoList;

    /// <summary>
    /// 根据输入的卡牌ID获取对应的卡牌信息
    /// </summary>
    /// <param name="id">卡牌ID</param>
    /// <returns></returns>
    public CardInfo GetInfo(CardID id) => CardInfoDic[id];

    /// <summary>
    /// 获取打乱的随机的卡牌ID数组
    /// </summary>
    /// <param name="num">随机数量</param>
    /// <returns></returns>
    public List<CardID> GetRandomIDs(int num)
    {
        var list = new List<CardID>(CardInfoDic.Keys);
        list.Shuffle();
        return list.GetRange(0,num);
    }
}
