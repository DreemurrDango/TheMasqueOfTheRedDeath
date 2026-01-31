using System;
using UnityEngine;

namespace Enums
{
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
        蓝色 = 0,
        紫红 = 1,
        绿色 = 2,
        橙黄 = 3,
        白色 = 4,
        紫色 = 5,
        黑色 = 6,
        COUNT
    }

    /// <summary>
    /// 所属阵营
    /// </summary>
    public enum Affiliation
    {
        红死魔,
        贵族,
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
}