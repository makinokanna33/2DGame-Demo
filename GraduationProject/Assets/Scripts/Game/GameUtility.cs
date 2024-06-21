using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtility
{
    #region 射线
    public static RaycastHit2D RayCheck(Vector3 origin, float length , Vector2 direction, LayerMask layerMask)
    {
        RaycastHit2D hit2D = Physics2D.Raycast(origin, direction, length, layerMask);

#if UNITY_EDITOR
        // 射线颜色设置
        Color color = hit2D ? Color.red : Color.green;
        // 绘画射线
        Debug.DrawRay(origin, direction * length, color);
#endif
        return hit2D;
    }

    public static bool RayCheckCircle(Vector3 center, float radius, LayerMask layerMask)
    {
        Collider2D collider = Physics2D.OverlapCircle(center, radius, LayerMask.GetMask("Player"));
        return collider != null;
    }
    #endregion

    public static GameObject LoadGameObject(string path)
    {
        string name = path.Substring(path.LastIndexOf('/') + 1, path.Length - path.LastIndexOf('/') - 1);
        GameObject obj = PoolManager.Instance.Get<GameObject>(name);
        if (obj == null)
        {
            obj = ResourcesManager.Instance.LoadAssetSync<GameObject>(path);
            obj = Object.Instantiate(obj);
        }
        else
        {
            obj.SetActive(true);
        }
        obj.name = name;
        return obj;
    }
}
