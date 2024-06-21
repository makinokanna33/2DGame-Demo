using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T: SingletonMono<T>
{
    protected static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if(instance!= null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this as T;
        DontDestroyOnLoad(this);
    }
}
