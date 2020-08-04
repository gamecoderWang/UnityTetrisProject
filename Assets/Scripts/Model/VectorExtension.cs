using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtension 
{
    /// <summary>
    /// 扩展方法，在Vector3对象中使用，传入Vector3,并截取x和y，并取它们间隔最近的整数
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public static Vector2 Round(this Vector3 v)
    {
        int x = Mathf.RoundToInt(v.x);
        int y = Mathf.RoundToInt(v.y);
        return new Vector2(x, y);
    }
}
