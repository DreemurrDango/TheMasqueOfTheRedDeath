using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.UI;

/// <summary>
/// 房间实例对象
/// </summary>
public class Room : MonoBehaviour
{
    [SerializeField]
    [Tooltip("房间枚举ID")]
    private RoomID roomID;
    [SerializeField]
    [Tooltip("连接其他房间的门列表")]
    private List<DoorUI> doors;
    [SerializeField]
    [Tooltip("房间内卡牌UI实例的根布局组")]
    private LayoutGroup roomCardGroup;
    [SerializeField]
    [Tooltip("卡牌槽位实例原型")]
    private GameObject cardSlotPrefabGO;
    [SerializeField]
    [Tooltip("房间内放置卡牌实例的缩放大小")]
    private float cardScale = 0.6f;

    /// <summary>
    /// 当前所有放置在此房间中的卡牌实例
    /// </summary>
    private List<CardUI> cards;
}
