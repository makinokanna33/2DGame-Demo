using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingletonMono<PoolManager>
{
    private Dictionary<string, Stack<Object>> poolDic = new Dictionary<string, Stack<Object>>();

    public void Push(string name, Object obj)
    {
        if (!poolDic.ContainsKey(name))
        {
            poolDic[name] = new Stack<Object>();
        }

        if(obj is GameObject)
        {
            (obj as GameObject).SetActive(false);
            (obj as GameObject).transform.SetParent(transform);
        }

        poolDic[name].Push(obj);
    }

    public T Get<T>(string name) where T:Object
    {
        if (poolDic.ContainsKey(name))
        {
            T obj = poolDic[name].Pop() as T;
            if (poolDic[name].Count == 0)
                poolDic.Remove(name);
            return obj;
        }

        return null;
    }
}
