using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Model : MonoBehaviour
{
    private int currentScore = 0;
    private int highScore = 0;
    private int gameNumbers = 0;

    public int CurrentScrore { get { return currentScore; } }
    public int HighScore { get { return highScore; } }
    public int GameNumbers { get { return gameNumbers; } }

    private const int ROW = 23;
    private const int COLUMN = 10;

    private Transform[,] map = new Transform[COLUMN, ROW];

    private void Awake()
    {
        ReadData();
    }

    /// <summary>
    /// 传入一个方块的位置，检查该方块所有子方块当前所处的格子是否有方块占用
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public bool IsAvailable(Transform t)
    {
        foreach (Transform item in t)
        {
            if (item.CompareTag("Block"))
            {
                Vector2 temp = item.position.Round();
                if (IsOutOfLimit(temp)) return false;
                if (map[(int)temp.x, (int)temp.y] != null) return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 将方块的每一小格放到地图格子中
    /// </summary>
    /// <param name="tr">要放入的方块的Transform</param>
    public void PlaceShapeToMaps(Transform tr)
    {
        foreach (Transform item in tr)
        {
            if(item.CompareTag("Block"))
            {
                Vector2 vec = item.position.Round();
                map[(int)vec.x, (int)vec.y] = item;
            }
        }
    }

    /// <summary>
    /// 判断是否出界
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    public bool IsOutOfLimit(Vector2 vec)
    {
        return vec.x < 0 || vec.x >= COLUMN || vec.y < 0;
    }

    /// <summary>
    /// 检查从第min列到第max列，从下往上遍历，每一列中直到遇到第一个空格子，就去遍历下一列，最后看哪一列的第一个空格在最下面，就返回那一列的第一个空格所在的行
    /// </summary>
    public int CheckRow(int max, int min)
    {
        int minRow = 100;
        for (int i = min; i <= max; i++)
        {
            for(int t = 0; t <= ROW; t++)
            {
                if(map[i, t] == null)
                {
                    if (t < minRow)
                        minRow = t;
                    break;
                }
            }
        }
        return minRow;
    }

    /// <summary>
    /// 检查格子的min到max行是否有需要消除的行，如果有就将其消除
    /// </summary>
    public bool CheckMap(int min, int max)
    {
        bool b = false;
        for (int i = min; i <= max; i++)
        {
            if(CheckIsLineFull(i))
            {
                this.currentScore += 10;
                b = true;
                DeleteRow(i);                                  //删除满的那一行
                MoveDownRowsAbove(i + 1);               //把上一行及以上的所有行都往下移动一行
                i--;
            }
        }
        return b;
    }

    /// <summary>
    /// 检查第row行是否满了
    /// </summary>
    /// <param name="row"></param>
    /// <returns></returns>
    private bool CheckIsLineFull(int row)
    {
        for (int i = 0; i < COLUMN; i++)
        {
            if (map[i, row] == null) return false;
        }
        return true;
    }

    /// <summary>
    /// 删除第row行
    /// </summary>
    /// <param name="row"></param>
    private void DeleteRow(int row)
    {
        for (int i = 0; i < COLUMN; i++)
        {
            Destroy(map[i, row].gameObject);
            map[i, row] = null;
        }
    }

    /// <summary>
    /// 把第row行以上的行全部往下移动一行
    /// </summary>
    /// <param name="row"></param>
    private void MoveDownRowsAbove(int row)
    {
        for (int i = row; i < ROW; i++)
        {
            MoveDownRow(i);
        }
    }

    /// <summary>
    /// 把第row行往下移动一行
    /// </summary>
    /// <param name="row"></param>
    private void MoveDownRow(int row)
    {
        for (int i = 0; i < COLUMN; i++)
        {
            if(map[i, row])
            {
                map[i, row - 1] = map[i, row];
                map[i, row] = null;
                map[i, row - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    /// <summary>
    /// 检查地图外面的第一行，如果有方块说明游戏结束
    /// </summary>
    /// <returns></returns>
    public bool IsGameOver()
    {
        for(int i = 0;  i < COLUMN; i++)
        {
            if (map[i, 20] != null)
            {
                gameNumbers++;
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 清除地图上的所有方块，并将当前分数置零
    /// </summary>
    public void ClearMapAndData()
    {
        for (int i = 0; i < ROW; i++)
        {
            for (int t = 0; t < COLUMN; t++)
            {
                if (map[t, i] != null)
                    Destroy(map[t, i].gameObject);
            }
        }
        map = new Transform[COLUMN, ROW];
        currentScore = 0;
    }

    /// <summary>
    /// 清除所有数据
    /// </summary>
    public void ClearAllData()
    {
        currentScore = 0;
        highScore = 0;
        gameNumbers = 0;
    }

    /// <summary>
    /// 存储游戏数据到文本文件
    /// </summary>
    public void SaveData()
    {
        if (currentScore > highScore)
            highScore = currentScore;
        string data = this.highScore.ToString() + "\n"+ this.gameNumbers.ToString();
        string path = Application.dataPath + "/Resources/GameData.txt";
        File.WriteAllText(path, data);
    }

    /// <summary>
    /// 从文本文件读取游戏数据
    /// </summary>
    public void ReadData()
    {
        TextAsset ta = Resources.Load<TextAsset>("GameData");
        string path = Application.dataPath + "/Resources/GameData.txt";
        string[] stringData = File.ReadAllLines(path);
        int[] intData = new int[stringData.Length];
        for (int i = 0; i < stringData.Length; i++)
        {
            if(string.IsNullOrEmpty(stringData[i]) == false)
            {
                intData[i] = int.Parse(stringData[i]);
            }
        }
        this.highScore = intData[0];
        this.gameNumbers = intData[1];
        this.currentScore = 0;
    }

    
}
