using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
/// <summary>
/// 动画文本：将文本内容作为帧，以动画形式显示出来
/// </summary>
public class AnimationText : MonoBehaviour
{
    [Tooltip("固定文本参数")]
    public string[] strArgus;
    [Tooltip("文本格式化帧")]
    public string[] formatFrames;
    [Tooltip("每帧间隔时间")]
    public float frameInterval = 0.1f;

    /// <summary>
    /// 此游戏对象上的文本组件
    /// </summary>
    private TMP_Text tmp_Text;
    /// <summary>
    /// 当前帧索引
    /// </summary>
    private int currentFrame = 0;

    private void OnEnable()
    {
        tmp_Text ??= GetComponent<TMP_Text>();
        StartCoroutine(PlayAnimation());

        IEnumerator PlayAnimation()
        {
            while (formatFrames.Length > 0)
            {
                tmp_Text.text = string.Format(formatFrames[currentFrame], strArgus);
                currentFrame = (currentFrame + 1) % formatFrames.Length;
                yield return new WaitForSeconds(frameInterval);
            }
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 设置显示的文本
    /// </summary>
    /// <param name="text">文本内容</param>
    /// <param name="playAnimation">是否播放文本动画</param>
    public void SetText(string text, bool playAnimation)
    {
        tmp_Text ??= GetComponent<TMP_Text>();
        tmp_Text.text = text;
        strArgus[0] = text;
        enabled = playAnimation;
    }
}
