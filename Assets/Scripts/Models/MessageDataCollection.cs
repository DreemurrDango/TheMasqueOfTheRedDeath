using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataCollection;
using Enums;
using System.Net;

namespace DreemurrStudio.Network
{
    #region C/S架构消息模板
    //WORKFLOW: 添加服务器发送给客户端的网络消息的数据结构
    public enum S2CMessageType
    {
        JoinLobby,
        ExitLobby,
    }

    // WORKFLOW: 添加客户端发送给服务器的网络消息的数据结构
    public enum C2SMessageType
    {
        JoinLobby,
        ExitLobby,
    }


    //WORKFLOW: 需要添加服务器发送给客户端的网络消息数据结构时，添加继承自此基类的新数据结构类
    [System.Serializable]
    /// <summary>
    /// 网络消息数据的基类
    /// </summary>
    public class S2CMessageDataBase
    {
        public S2CMessageType messageType;
    }

    //WORKFLOW: 需要添加客户端发送给服务器的网络消息数据结构时，添加继承自此基类的新数据结构类
    /// <summary>
    /// 客户端向服务器发送的网络消息数据的基类
    /// </summary>
    [System.Serializable]
    public class C2SMessageDataBase
    {
        public C2SMessageType messageType;
    }
    #endregion

    /// <summary>
    /// 消息类型枚举
    /// </summary>
    public enum MessageType
    {
        S2C_RoomPlayerUpdated,
        COUNT
    }

    /// <summary>
    /// 消息的基类
    /// </summary>
    [System.Serializable]
    public class MessageDataBase
    {
        /// <summary>
        /// 消息类型，根据此字段判断消息数据结构的具体类型
        /// </summary>
        public MessageType messageType;

        /// <summary>
        /// 将当前消息数据序列化为JSON字符串
        /// </summary>
        /// <returns></returns>
        public string ToJson() => JsonConvert.SerializeObject(this);
    }

    /// <summary>
    /// 服务器发送给客户端的房间玩家更新消息
    /// </summary>
    [System.Serializable]
    public class S2C_RoomPlayerUpdated : MessageDataBase
    {
        public Dictionary<IPEndPoint,PlayerID> playerIPIDDict;
    }
}