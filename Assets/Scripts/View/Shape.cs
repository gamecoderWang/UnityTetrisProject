using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    private bool isPause = false;

    private Transform pivot;

    private float time = 0;

    private float stepTime = 0.8f;

    private Control ctrl;

    private GameController gameController;

    private void Awake()
    {
        pivot = transform.Find("Pivot");
    }

    void Update()
    {
        if (isPause) return;
        InputControl();
        time += Time.deltaTime;
        if (time >= stepTime)
        {
            Fall();
            time = 0;
        }
        
    }

    public void Init(Color color, Control ctrl, GameController gamectrl)
    {
        foreach (Transform item in transform)
        {
            if(item.CompareTag("Block"))
            {
                Color blockColor = item.GetComponent<SpriteRenderer>().color;
                blockColor = color;
                blockColor.a = 255;
                item.GetComponent<SpriteRenderer>().color = blockColor;
            }
        }
        this.ctrl = ctrl;
        gameController = gamectrl;
    }

    /// <summary>
    /// 物块下落
    /// </summary>
    private void Fall()
    {
        Vector3 fall = transform.position;
        fall.y -= 1;
        transform.position = fall;
        if(ctrl.model.IsAvailable(this.transform) == false)
        {
            fall.y += 1;
            transform.position = fall;
            isPause = true;
            this.ctrl.model.PlaceShapeToMaps(this.transform);
            ctrl.gameController.CheckGameOver();
            gameController.CheckDelete();
            this.gameController.ResetCurrentShape();
        }
        
        ctrl.audioManager.PlayShapeFallAudio();
        
    }

    public void PauseShapeFall()
    {
        isPause = true;
    }

    public void StartShapeFall()
    {
        isPause = false;
    }

    /// <summary>
    /// 控制交互
    /// </summary>
    private void InputControl()
    {
        int h = 0;
        if (Input.GetKeyDown(KeyCode.A))
            h = -1;
        if (Input.GetKeyDown(KeyCode.D))
            h = 1;
        if(h != 0)
        {
            Vector3 tempVector = this.transform.position;
            tempVector.x += h;
            this.transform.position = tempVector;
            if(ctrl.model.IsAvailable(this.transform) == false)
            {
                tempVector.x -= h;
                this.transform.position = tempVector;
            }
            else
            ctrl.audioManager.PlayShapeActionAudio();
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            transform.RotateAround(pivot.position, transform.forward, -90);
            if(ctrl.model.IsAvailable(this.transform) == false)
            {
                transform.RotateAround(pivot.position, transform.forward, 90);
            }
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            int maxCol;
            int minCol;
            CheckRow(out maxCol, out minCol);                                               //首先检查该方块从左到右占了几列
            int minAvailableCol = ctrl.model.CheckRow(maxCol, minCol);              //从下到上遍历这几列的每一个格子，
            //下面这三行先将方块挪到这几列中空格子最低的地方
            Vector3 temp = transform.position;
            temp.y = minAvailableCol;
            transform.position = temp;
            //再检查这个地方有没有被占用，如果true，就将方块往上挪一各，然后再检查，如果还不行，再往上挪，知道能放下为止
            while(!ctrl.model.IsAvailable(this.transform))
            {
                temp.y += 1;
                transform.position = temp;
            }
            ctrl.model.PlaceShapeToMaps(this.transform);                //将当前方块放入地图格子中
            ctrl.gameController.CheckGameOver();                    //检查游戏是否结束
            gameController.CheckDelete();                               //检查是否有行要消除
            gameController.ResetCurrentShape();                     //重置当前方块的引用，使GameController的update函数中能继续产生新方块
            isPause = true;                                                 //暂停该方块的行为，使该方块Update函数停止执行，这一步如果没有，从第一个方块下落后每次按下S键会同时掉落两个方块
            time = 0;
        }
    }

    //检查该方块从左到右占了哪几列
    private void CheckRow(out int max, out int min)
    {
        max = -1; min = 100;
        foreach (Transform item in this.transform)
        {
            if(item.CompareTag("Block"))
            {
                Vector2 temp = item.position.Round();
                if (temp.x >= max)
                    max = (int)temp.x;
                if (temp.x <= min)
                    min = (int)temp.x;
            }
        }
    }

    /// <summary>
    /// 计算这个方块占了几行
    /// </summary>
    /// <param name="max">该方块最上面是第几行</param>
    /// <param name="min">该方块最下面是第几行，</param>
    public void CheckColumn(out int max, out int min)
    {
        max = -1; min = 100;
        foreach (Transform item in this.transform)
        {
            if (item.CompareTag("Block"))
            {
                Vector2 temp = item.position.Round();
                if (temp.y >= max)
                    max = (int)temp.y;
                if (temp.y <= min)
                    min = (int)temp.y;
            }
        }
    }

}
