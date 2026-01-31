using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 连接房间的门
/// </summary>
public class DoorUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("卡牌槽位游戏对线")]
    private GameObject cardSlotGO;
    [SerializeField]
    [Tooltip("可放置提示显示对象")]
    private GameObject placeableTipShowGO;
    [SerializeField]
    [Tooltip("卡牌UI预制体")]
    private CardUI cardUIPrefab;
    [SerializeField]
    [Tooltip("连接的房间列表")]
    private List<Room> connetedRooms;

    /// <summary>
    /// 当前门上放置的卡牌UI实例
    /// </summary>
    private CardUI cardUI;
}
