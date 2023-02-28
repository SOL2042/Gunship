using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    #region Singleton
    private static EventManager _instance;
    public static EventManager instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<EventManager>();
            return _instance;
        }
    }
    #endregion

    public delegate void OnEvent(object param);
    public Dictionary<string, OnEvent> eventList = new Dictionary<string, OnEvent>();

    public void AddListener(string key, OnEvent func)
    {
        if (!eventList.TryGetValue(key, out OnEvent e))
        {
            eventList.Add(key, func);
        }
    }

    public void PostEvent(string key, object value)
    {
        if (eventList.TryGetValue(key, out OnEvent e))
        {
            e.Invoke(value);
        }
    }
}