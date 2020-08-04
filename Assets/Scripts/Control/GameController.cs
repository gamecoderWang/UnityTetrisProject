using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    private bool isPause = true;                 //控制是否持续生成方块
    private Shape currentShape = null;                  //当前正在下落的方块

    public Color[] colors;                                      //方块所有颜色集合
    public Shape[] shapes;                                  //所有方块集合

    private Transform shapePoint;                    //存放方块的游戏物体，游戏中生成的方块全部是它的子物体

    private Control ctrl;

    // Start is called before the first frame update
    void Awake()
    {
        shapePoint = GameObject.Find("View/Map/ShapePoint").GetComponent<Transform>();
        ctrl = GetComponent<Control>();
    }

    private void Start()
    {
        InvokeRepeating("ClearNullShape", 30f, 30f);        //每隔三十秒钟把没有子物体的空方块清理一次
    }

    void Update()
    {
        if (isPause) return;
        if (currentShape == null)               //当前没有正在下落的方块就生成新方块
        {
            SpawnShape();     
        }
    }

    /// <summary>
    /// 生成新方块
    /// </summary>
    private void SpawnShape()
    {
        int sharpIndex = Random.Range(0, shapes.Length);
        int colorIndex = Random.Range(0, colors.Length);
        currentShape = Instantiate(shapes[sharpIndex]);
        currentShape.transform.SetParent(shapePoint);
        currentShape.transform.localPosition = Vector3.zero;
        currentShape.Init(colors[colorIndex], ctrl, this);
    }

    /// <summary>
    /// 开始游戏，方块开始生成，开始正常下落
    /// </summary>
    public void StartGame()
    {
        isPause = false;
        if (currentShape != null)
            currentShape.StartShapeFall();
    }

    /// <summary>
    /// 暂停游戏
    /// </summary>
    public void PauseGame()
    {
        isPause = true;
        if (currentShape != null)
            currentShape.PauseShapeFall();
    }

    /// <summary>
    /// 将当前正在下落的方块置空
    /// </summary>
    public void ResetCurrentShape()
    {
        currentShape = null;
    }

    /// <summary>
    /// 检查是否有行需要消除
    /// </summary>
    public void CheckDelete()
    {
        int maxRow;
        int minRow;
        currentShape.CheckColumn(out maxRow, out minRow);
        bool b = ctrl.model.CheckMap(minRow, maxRow);
        if (b)
        {
            ctrl.audioManager.PlayLineClearAudio();
            ctrl.view.UpdateScore(ctrl.model.CurrentScrore);
        }
    }

    /// <summary>
    /// 检查游戏是否结束
    /// </summary>
    public void CheckGameOver()
    {
        if (ctrl.model.IsGameOver())
        {
            PauseGame();
            ctrl.view.ShowGameOverUI(ctrl.model.CurrentScrore);
            ctrl.model.SaveData();
        }
    }

    /// <summary>
    /// 清楚所有无用的空方块，Start里面开始每隔30秒调用一次
    /// </summary>
    private void ClearNullShape()
    {
        foreach (Transform item in shapePoint)
        {
            if(item.childCount <= 1)
            {
                Destroy(item.gameObject);
            }
        }
    }

    /// <summary>
    /// 清除地图上的所有方块，并将当前方块置空
    /// </summary>
    public void ClearAllShape()
    {
        ctrl.model.ClearMapAndData();
        if (currentShape != null)
            Destroy(currentShape.gameObject);
        ResetCurrentShape();
    }
}
