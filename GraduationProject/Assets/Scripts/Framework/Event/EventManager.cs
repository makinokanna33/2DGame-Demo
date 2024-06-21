using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// EventManager system.
/// </summary>
public class EventManager
{
    #region 属性
    // 存储所有注册好的事件以及对应的key值
    private static Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>();
    // 存储所有注册好的事件以及对应的发送者
    private static Dictionary<string, object> sender = new Dictionary<string, object>();
    // 存储所有注册好的事件以及是否正在被监听
    private static Dictionary<string, bool> paused = new Dictionary<string, bool>();

    // 存储的是事件对应的参数object类型
    private static Dictionary<string, object> storage = new Dictionary<string, object>();

    // 存储callbcakID和其对应的回调函数
    private static Dictionary<string, UnityAction> callBacks = new Dictionary<string, UnityAction>();

    // 过滤条件
    private struct SFilter
    {
        public string value;
        public bool starts;
        public bool ends;
        public bool contains;
        public bool exact;
    }

    #endregion 属性

    #region 开始监听

    /// <summary>
    /// 开始监听
    /// </summary>
    /// <param name="eventName">事件名</param>
    /// <param name="callBack">回调函数</param>
    /// <param name="callBackID">唯一的回调函数的ID</param>
    public static void StartListening(string eventName, UnityAction callBack, string callBackID = "")
    {
        if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.AddListener(callBack);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(callBack);
            eventDictionary.Add(eventName, thisEvent);
            paused.Add(eventName, false);
        }

        if (callBackID != "") callBacks.Add(eventName + "_" + callBackID, callBack);
    }

    /// <summary>
    /// 开始监听事件，这里为触发事件时使用过滤器做准备
    /// </summary>
    /// <param name="eventName">事件名，具备唯一性</param>
    /// <param name="target">指定的触发对象，可通过该对象的一些属性（name,tag...）来指定触发事件</param>
    /// <param name="callBack">回调函数</param>
    /// <param name="callBackID">回调函数的ID，具备唯一性</param>
    public static void StartListening(string eventName, GameObject target, UnityAction callBack, string callBackID = "")
    {
        if (target == null)
        {
            Debug.LogError("指定的target不是一个有效的游戏对象");
            return;
        }

        StartListening(eventName, callBack);
        if (callBackID != "") callBacks.Add(eventName + "_" + callBackID, callBack);

        string newName = eventName + "__##name##" + target.name + "##" + "__##tag##" + target.tag + "##" + "__##layer##" + target.layer + "##";
        StartListening(newName, callBack);
        if (callBackID != "") callBacks.Add(eventName + "_" + callBackID + "_EXTRA", callBack);
    }

    #endregion 开始监听

    #region 停止监听

    /// <summary>
    /// 通过callBackID名来停止监听，前提是必须在监听时为callBack添加了callBackID
    /// </summary>
    /// <param name="eventName">事件名，具备唯一性</param>
    /// <param name="callBackID">回调函数ID，具备唯一性</param>
    public static void StopListening(string eventName, string callBackID)
    {
        if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            if (callBackID != "")
            {
                if (callBacks.ContainsKey(eventName + "_" + callBackID))
                {
                    thisEvent.RemoveListener(callBacks[eventName + "_" + callBackID]);
                    callBacks.Remove(eventName + "_" + callBackID);
                }
                if (callBacks.ContainsKey(eventName + "_" + callBackID + "_EXTRA"))
                {
                    thisEvent.RemoveListener(callBacks[eventName + "_" + callBackID + "_EXTRA"]);
                    callBacks.Remove(eventName + "_" + callBackID + "_EXTRA");
                }
            }
        }
    }

    /// <summary>
    /// 通过回调函数名来停止监听
    /// </summary>
    /// <param name="eventName">事件名，具备唯一性</param>
    /// <param name="callBackID">回调函数</param>
    public static void StopListening(string eventName, UnityAction callBack)
    {
        if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.RemoveListener(callBack);
        }
    }

    #endregion 停止监听

    #region 事件触发

    /// <summary>
    /// 通过事件名触发事件
    /// </summary>
    /// <param name="eventName">事件名，具备唯一性</param>
    public static void EmitEvent(string eventName)
    {
        if (isPaused(eventName)) return;

        if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    /// <summary>
    /// 通过事件名触发一个事件，并记录发送者
    /// </summary>
    /// <param name="eventName">事件名，具备唯一性</param>
    /// <param name="sender">发送者</param>
    public static void EmitEvent(string eventName, object sender)
    {
        if (isPaused(eventName)) return;

        if (EventManager.sender.ContainsKey(eventName))
            EventManager.sender[eventName] = sender;
        else
            EventManager.sender.Add(eventName, sender);

        EmitEvent(eventName);
    }

    /// <summary>
    /// 延迟触发事件
    /// </summary>
    /// <param name="eventName">事件名，具备唯一性</param>
    /// <param name="delay">延迟时间，单位秒</param>
    public static void EmitEvent(string eventName, float delay)
    {
        if (isPaused(eventName)) return;

        if (eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
        {
            if (delay <= 0)
            {
#if UNITY_EDITOR
                if (thisEvent is null)
                    Debug.LogFormat("出现bug，事件{0}的回调函数为NULL，请检测错误出现原因", eventName);
#endif
                thisEvent?.Invoke();
            }
            else
            {
                int d = (int)(delay * 1000);
                DelayedInvoke(thisEvent, d);
            }
        }
    }

    /// <summary>
    /// 通过事件名延迟触发事件，并记录发送者
    /// </summary>
    /// <param name="eventName">事件名，具备唯一性</param>
    /// <param name="delay">延迟时间，单位秒</param>
    /// <param name="sender">发送者</param>
    public static void EmitEvent(string eventName, float delay, object sender)
    {
        if (isPaused(eventName)) return;

        if (EventManager.sender.ContainsKey(eventName)) EventManager.sender[eventName] = sender; else EventManager.sender.Add(eventName, sender);

        EmitEvent(eventName, delay);
    }

    /// <summary>
    ///  让指定名称的事件只触发指定目标（通过filter）的回调函数。可以指定延迟和发送者。
    ///  例如：name:sword;layer:UI 表示以name是sword且是UI层的物体才会触发事件
    ///  name:play*;tag:*hero* 表示的是tag包含"hero"且名字以"play"开始的物体
    /// </summary>
    /// <param name="eventName">事件名，具备唯一性</param>
    /// <param name="filter">过滤条件，满足条件的才会触发，通过;进行分割，*可以指示是否是包含、开始、结尾</param>
    /// <param name="delay">延迟时间，单位秒</param>
    /// <param name="sender">发送者</param>
    public static void EmitEvent(string eventName, string filter, float delay = 0f, object sender = null)
    {
        if (sender != null)
        {
            if (EventManager.sender.ContainsKey(eventName)) EventManager.sender[eventName] = sender; else EventManager.sender.Add(eventName, sender);
        }

        // Extract filter data.
        var data = filter.Split(';');
        var filters = new Dictionary<string, SFilter>();

        foreach (string s in data)
        {
            var tmp = s.Split(':');
            if (tmp[0] == "name")
                filters.Add("name", new SFilter
                {
                    value = tmp[1].Replace("*", ""),
                    contains = tmp[1].StartsWith("*") && tmp[1].EndsWith("*"),
                    starts = tmp[1].EndsWith("*"),
                    ends = tmp[1].StartsWith("*"),
                    exact = !tmp[1].Contains("*")
                });
            else if (tmp[0] == "tag")
                filters.Add("tag", new SFilter
                {
                    value = tmp[1].Replace("*", ""),
                    contains = tmp[1].StartsWith("*") && tmp[1].EndsWith("*"),
                    starts = tmp[1].EndsWith("*"),
                    ends = tmp[1].StartsWith("*"),
                    exact = !tmp[1].Contains("*")
                });
            else if (tmp[0] == "layer")
                filters.Add("layer", new SFilter
                {
                    value = tmp[1].Replace("*", ""),
                    contains = tmp[1].StartsWith("*") && tmp[1].EndsWith("*"),
                    starts = tmp[1].EndsWith("*"),
                    ends = tmp[1].StartsWith("*"),
                    exact = !tmp[1].Contains("*")
                });
        }

        int counter = filters.Count;
        int found = 0;

        // 在所有事件中寻找符合条件的目标

        foreach (KeyValuePair<string, UnityEvent> evnt in eventDictionary)
        {
            var key = evnt.Key;

            if (key.Contains("_") && key.StartsWith(eventName))
            {
                data = key.Split('_');

                var name = "";
                var tag = "";
                var layer = "";

                found = 0;

                foreach (string s in data)
                {
                    if (s.Contains("##name##")) name = s.Replace("##name##", "").Replace("#", "");
                    if (s.Contains("##tag##")) tag = s.Replace("##tag##", "").Replace("#", "");
                    if (s.Contains("##layer##")) layer = s.Replace("##layer##", "").Replace("#", "");
                }

                if (filters.ContainsKey("name") && name != "")
                {
                    if (FilterIsValidated(name, filters["name"])) found++;
                }
                if (filters.ContainsKey("tag") && tag != "")
                {
                    if (FilterIsValidated(tag, filters["tag"])) found++;
                }
                if (filters.ContainsKey("layer") && layer != "")
                {
                    if (FilterIsValidated(layer, filters["layer"])) found++;
                }

                if (found == counter) { EmitEvent(key, delay); }
            }
        }
    }

    //过滤器验证
    private static bool FilterIsValidated(string value, SFilter rules)
    {
        if (rules.exact)
        {
            return value == rules.value;
        }
        else if (rules.contains)
        {
            return value.Contains(rules.value);
        }
        else if (rules.starts)
        {
            return value.StartsWith(rules.value);
        }
        else if (rules.ends)
        {
            return value.EndsWith(rules.value);
        }
        return false;
    }

    /// <summary>
    /// 触发事件并且传值
    /// </summary>
    public static void EmitEventData(string eventName, object data, float delay = 0f)
    {
        SetData(eventName, data);
        EmitEvent(eventName, delay);
    }

    #endregion 事件触发

    #region 工具方法

    /// <summary>
    /// 停止所有监听
    /// </summary>
    public static void StopAll()
    {
        foreach (KeyValuePair<string, UnityEvent> evnt in eventDictionary)
        {
            evnt.Value.RemoveAllListeners();
        }
        eventDictionary = new Dictionary<string, UnityEvent>();
    }

    private static async void DelayedInvoke(UnityEvent thisEvent, int delay)
    {
        await Task.Delay(delay);
        thisEvent?.Invoke();
    }

    /// <summary>
    /// 判断是否事件系统是否正在监听事件
    /// </summary>
    /// <returns></returns>
    public static bool IsListening()
    {
        return eventDictionary.Count > 0;
    }

    /// <summary>
    /// 暂停全部监听
    /// </summary>
    public static void PauseListening()
    {
        SetPaused(true);
    }

    /// <summary>
    /// 暂停某个事件的监听
    /// </summary>
    /// <param name="eventName">事件名，具备唯一性</param>
    public static void PauseListening(string eventName)
    {
        SetPaused(eventName, true);
    }

    /// <summary>
    ///重启所有监听
    /// </summary>
    public static void RestartListening()
    {
        SetPaused(false);
    }

    /// <summary>
    /// 重启某个事件的监听
    /// </summary>
    /// <param name="eventName">>事件名，具备唯一性</param>
    public static void RestartListening(string eventName)
    {
        SetPaused(eventName, false);
    }

    /// <summary>
    /// 判断一个事件是否正在监听
    /// </summary>
    /// <param name="eventName">事件名，具备唯一性</param>
    /// <returns></returns>
    public static bool isPaused(string eventName)
    {
        if (paused.ContainsKey(eventName)) return paused[eventName]; else return true;
    }

    private static void SetPaused(bool value)
    {
        Dictionary<string, bool> copy = new Dictionary<string, bool>();

        foreach (KeyValuePair<string, bool> eName in paused)
        {
            copy.Add(eName.Key, value);
        }

        paused = copy;
    }

    private static void SetPaused(string eventName, bool value)
    {
        Dictionary<string, bool> copy = new Dictionary<string, bool>();

        foreach (KeyValuePair<string, bool> eName in paused)
        {
            if (eName.Key == eventName) copy.Add(eName.Key, value); else copy.Add(eName.Key, eName.Value);
        }

        paused = copy;
    }

    /// <summary>
    /// 判断事件是否存在
    /// </summary>
    public static bool EventExists(string eventName)
    {
        return eventDictionary.ContainsKey(eventName);
    }

    /// <summary>
    /// 清理某个事件对应的参数缓存，仍然会继续监听
    /// </summary>
    /// <param name="eventName"></param>
    public static void Dispose(string eventName)
    {
        if (storage.ContainsKey(eventName)) storage.Remove(eventName);
    }

    /// <summary>
    /// 清理全部的参数缓存
    /// </summary>
    public static void DisposeAll()
    {
        storage.Clear();
        sender.Clear();
    }

    #endregion " UTILS METHODS "

    #region 获取设置数据

    /// <summary>
    /// 设置事件的传参
    /// </summary>
    public static void SetData(string eventName, object data)
    {
        if (storage.ContainsKey(eventName)) storage[eventName] = data; else storage.Add(eventName, data);
    }

    /// <summary>
    /// 获取数据
    /// </summary>
    /// <typeparam name="T">数据的类型，会进行强转，失败返回null</typeparam>
    /// <param name="eventName">事件名，唯一性</param>
    /// <returns></returns>
    public static T GetData<T>(string eventName)
    {
        try
        {
            if (storage.ContainsKey(eventName)) return (T)storage[eventName]; else return default;
        }
        catch (System.Exception)
        {
            return default;
        }
    }

    /// <summary>
    /// 获取事件的发送者，在发送时设置，没有设置返回null
    /// </summary>
    /// <param name="eventName">事件名，唯一性</param>
    /// <returns></returns>
    public static object GetSender(string eventName)
    {
        try
        {
            if (sender.ContainsKey(eventName)) return sender[eventName]; else return null;
        }
        catch (System.Exception)
        {
            return null;
        }
    }

    #endregion 获取设置数据
}

#region 事件组

/// <summary>
/// 创建一组事件，可以设置开始和暂停
/// </summary>
public class EventsGroup
{
    private struct SEvent
    {
        public string name;
        public UnityAction callBack;
    }

    private List<SEvent> group = new List<SEvent>();

    /// <summary>
    /// 添加新的事件到事件组中
    /// </summary>
    public void Add(string eventName, UnityAction callBack)
    {
        group.Add(new SEvent { name = eventName, callBack = callBack });
    }

    /// <summary>
    /// 开始监听
    /// </summary>
    public void StartListening()
    {
        foreach (SEvent g in group)
        {
            EventManager.StartListening(g.name, g.callBack);
        }
    }

    /// <summary>
    /// 停止监听事件，如果指定了eventName则只停止该事件
    /// </summary>
    public void StopListening(string eventName = "")
    {
        if (eventName == "")
        {
            foreach (SEvent g in group)
            {
                EventManager.StopListening(g.name, g.callBack);
            }
        }
        else
        {
            List<SEvent> newGroup = new List<SEvent>();
            foreach (SEvent g in group)
            {
                if (g.name != eventName) newGroup.Add(g); else EventManager.StopListening(g.name, g.callBack);
            }
            group = newGroup;
        }
    }

    /// <summary>
    /// 查看事件组是否包含该事件
    /// </summary>
    public bool Contains(string eventName)
    {
        foreach (SEvent g in group)
        {
            if (g.name == eventName) return true;
        }
        return false;
    }
}

#endregion 事件组
