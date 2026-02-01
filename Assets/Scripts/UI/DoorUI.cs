using DataCollection;
using Enums;
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
    [Tooltip("可放置提示显示对象")]
    private GameObject interactableTipShowGO;
    [SerializeField]
    [Tooltip("卡牌UI预制体")]
    private CardUI cardUI;
    [SerializeField]
    [Tooltip("连接的房间列表")]
    private List<Room> connetedRooms;

    /// <summary>
    /// 获取其中的卡牌信息
    /// </summary>
    public CardID? CardID
    {
        get => cardUI.gameObject.activeSelf ? cardUI.ID : null;
        set
        {
            if (value != null) cardUI.Init(value.Value, CardState.InDoor);
            cardUI.gameObject.SetActive(value.HasValue);
        }
    }

    /// <summary>
    /// 设置是否显示可放置提示
    /// </summary>
    public bool ShowInteractableTip
    {
        get => interactableTipShowGO.activeSelf;
        set => interactableTipShowGO.SetActive(value);
    }

    public void InitClear()
    {
        ShowInteractableTip = false;
        cardUI.gameObject.SetActive(false);
        connetedRooms = new List<Room>();
    }
}
