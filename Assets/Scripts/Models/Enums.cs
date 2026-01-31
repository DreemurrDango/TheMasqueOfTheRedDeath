using System;
using UnityEngine;

namespace Enums
{
    /// <summary>
    /// 玩家ID枚举
    /// </summary>
    public enum PlayerID
    {
        玩家1 = 0,
        玩家2 = 1,
        玩家3 = 2,
        玩家4 = 3,
        COUNT
    }

    /// <summary>
    /// 角色卡枚举
    /// </summary>
    public enum CardID
    {
        红死魔,
        邪教徒,
        窃贼,
        刺客,
        亲王,
        管家,
        侍从,
        侍卫队长,
        侍卫,
        瘟疫医生,
        乐师,
        贵族,
        弄臣,
        吟游诗人,
        舞者
    }

    /// <summary>
    /// 房间ID枚举
    /// </summary>
    public enum RoomID
    {
        蓝色房间 = 0,
        紫红房间 = 1,
        绿色房间 = 2,
        橙黄房间 = 3,
        白色房间 = 4,
        紫色房间 = 5,
        黑色房间 = 6,
        COUNT
    }

    /// <summary>
    /// 所属阵营
    /// </summary>
    public enum Affiliation
    {
        红死魔,
        宾客,
        特殊_窃贼,
        特殊_刺客
    }

    [Flags]
    /// <summary>
    /// 卡牌使用方式
    /// </summary>
    public enum CardUsageType
    {
        发动效果 = 1 << 0,
        封锁走廊 = 1 << 1,
        盖卡放置 = 1 << 2
    }

    /// <summary>
    /// 卡牌位置状态
    /// </summary>
    public enum CardState
    {
        //卡牌未在本局游戏中使用
        NONE,
        /// <summary>
        /// 卡牌在手中
        /// </summary>
        InHand,
        /// <summary>
        /// 卡牌已因被使用而从游戏中除外
        /// </summary>
        Used,
        /// <summary>
        /// 卡牌被放置在房间中
        /// </summary>
        InRoom,
        /// <summary>
        /// 卡牌被放置在门中用于封锁走廊
        /// </summary>
        InDoor
    }
}