using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    /// <summary>
    /// 实例化对象
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static GameObject Instantiate(GameObject obj, Transform parent)
    {
        GameObject result = GameObject.Instantiate(obj, parent) as GameObject;
        ResetTransform(result, obj);
        return result;
    }

    /// <summary>
    /// 重置Transform信息
    /// </summary>
    /// <param name="current"></param>
    /// <param name="other"></param>
    public static void ResetTransform(GameObject current, GameObject other)
    {
        current.transform.localPosition = other.transform.localPosition;
        current.transform.localRotation = other.transform.localRotation;
        current.transform.localScale = other.transform.localScale;
        if (current.transform is RectTransform && other.transform is RectTransform)
        {
            RectTransform currentRect = current.transform as RectTransform;
            RectTransform otherRect = other.transform as RectTransform;
            currentRect.offsetMin = otherRect.offsetMin;
            currentRect.offsetMax = otherRect.offsetMax;
            currentRect.sizeDelta = otherRect.sizeDelta;
        }
    }

    #region Enum

    public static T ToEnum<T>(this string enumStr)
    {
        return (T)Enum.Parse(typeof(T), enumStr);
    }

    public static List<string> GetEnumStrList(Type enumtype)
    {
        if (!enumtype.IsEnum)
            return null;
        return new List<string>(Enum.GetNames(enumtype));
    }

    public static List<string> GetEnumIntList(Type enumtype)
    {
        if (!enumtype.IsEnum)
            return null;
        return new List<string>(Enum.GetNames(enumtype));
    }

    #endregion Enum
}