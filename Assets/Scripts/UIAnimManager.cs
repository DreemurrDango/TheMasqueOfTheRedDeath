using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI动画管理器
/// 在这里调用UI组件的动画播放，监听动画完成事件回调
/// </summary>
public class UIAnimManager : MonoBehaviour
{
    /// <summary>
    /// 当前是否处于动画播放中
    /// </summary>
    private bool inAnimation = false;
    /// <summary>
    /// 当前是否处于动画播放中
    /// </summary>
    public bool InAnimation { get { return inAnimation; } }
}
