using UnityEngine;
using Enums;
using System.Collections.Generic;

//存放在项目中普遍使用的自定义数据类型，一般为struct
namespace DataCollection
{
    /// <summary>
    /// WORKFLOW:数据结果模板
    /// </summary>
    [System.Serializable]
    public struct GameConfig
    {
    }

    [System.Serializable]
    /// <summary>
    /// 卡牌模板信息
    /// </summary>
    public class CardInfo
    {
        /// <summary>
        /// 卡牌身份枚举
        /// </summary>
        public CardID card;
        /// <summary>
        /// 卡牌角色画像图片
        /// </summary>
        public Sprite portrait;
        /// <summary>
        /// 卡牌能力说明
        /// </summary>
        public string abilityDesc;
        /// <summary>
        /// 胜利条件文本
        /// </summary>
        public string victoryCondition;
        /// <summary>
        /// 所属阵营
        /// </summary>
        public Affiliation affiliation;
        /// <summary>
        /// 卡牌的使用方式
        /// </summary>
        public CardUsageType usageType;

        /// <summary>
        /// 获取卡牌的显示名称
        /// </summary>
        public string CardName => card.ToString();
        /// <summary>
        /// 阵营显示名称
        /// </summary>
        public string AffiliationName => affiliation.ToString();
    }
}