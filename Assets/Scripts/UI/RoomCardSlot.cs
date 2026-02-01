using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCardSlot : MonoBehaviour
{
    [SerializeField]
    private CardUI card;
    /// <summary>
    /// 其中的卡牌UI
    /// </summary>
    public CardUI Card { get { return card; } }
}
