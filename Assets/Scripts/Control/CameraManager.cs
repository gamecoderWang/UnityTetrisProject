using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 控制摄像机放大与缩小的动画
/// </summary>
public class CameraManager : MonoBehaviour
{
    private Camera mainCamera;

    // Start is called before the first frame update
    void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ZoomIn()
    {
        mainCamera.DOOrthoSize(13.3f, 0.5f);
    }

    public void ZoomOut()
    {
        mainCamera.DOOrthoSize(15.59f, 1f);
    }
}
