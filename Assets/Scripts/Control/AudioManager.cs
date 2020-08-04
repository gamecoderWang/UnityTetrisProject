using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public bool isMute = false;

    public AudioClip mouseClick;

    public AudioClip shapeFall;

    public AudioClip shapeAction;

    public AudioClip lineClear;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 方块自动下落时播放
    /// </summary>
    public void PlayShapeFallAudio()
    {
        PlayAudio(mouseClick);
    }

    /// <summary>
    /// 点击鼠标时播放
    /// </summary>
    public void PlayMouseClickAudio()
    {
        PlayAudio(shapeFall);
    }

    /// <summary>
    /// 左右移动时播放
    /// </summary>
    public void PlayShapeActionAudio()
    {
        PlayAudio(shapeAction);
    }

    /// <summary>
    /// 整行消除时播放
    /// </summary>
    public void PlayLineClearAudio()
    {
        PlayAudio(lineClear);
    }

    /// <summary>
    /// 播放声音
    /// </summary>
    /// <param name="clip"></param>
    private void PlayAudio(AudioClip clip)
    {
        if (isMute) return;
        audioSource.clip = clip;
        audioSource.Play();
    }
}
