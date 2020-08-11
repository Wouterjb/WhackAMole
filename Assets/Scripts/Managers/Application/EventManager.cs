﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Event : UnityEvent<System.Object>
{
}

public class EventManager : MonoBehaviour
{
    // Enum
    public enum CustomEventType
    {
        EVENT_NONE = 0,
        EVENT_INIT_GAME = 1,
        EVENT_ACTIVATE_CANVAS = 2,
        EVENT_PLAYER_START = 3,
        EVENT_PLAYER_STOP = 4,
        EVENT_SESSION_START = 5,
        EVENT_SESSION_END = 6,
        EVENT_LOADING_STARTED = 7,
        EVENT_LOADING_COMPLETED = 8,
        EVENT_LOADING_PROGRESSED = 9,
        EVENT_NEW_HIGHSCORE = 10,
        EVENT_UPDATE_SESSION_TIME = 11,
        EVENT_SCORED_POINTS = 12,
        EVENT_TOTAL_SCORED_POINTS = 13,
    }

    // Singleton
    private static EventManager instance;

    // Bool
    private bool isInitialized = false;

    public static EventManager Instance
    {
        get { return instance; }
    }

    // Collections
    private Dictionary<CustomEventType, Event> eventLibrary;

    // Awake is called at initialization of this class
    public void Awake()
    {
        // Singleton creation
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<EventManager>();
        }
        else
        {
            Destroy(this);
            return;
        }

        isInitialized = true;
        eventLibrary = new Dictionary<CustomEventType, Event>();
    }

    public void AddListener(CustomEventType eventType, UnityAction<System.Object> listener)
    {
        // Check if already existing
        Event unityEvent = null;
        bool exists = eventLibrary.TryGetValue(eventType, out unityEvent);

        if (exists)
        {
            // Already exists, add listener to event
            unityEvent.AddListener(listener);
        } 
        else
        {
            // Create a new one
            unityEvent = new Event();
            unityEvent.AddListener(listener);
            eventLibrary.Add(eventType, unityEvent);
        }
    }

    public void RemoveListener(CustomEventType eventType, UnityAction<System.Object> listener)
    {
        // Check if already existing
        Event unityEvent = null;
        bool exists = eventLibrary.TryGetValue(eventType, out unityEvent);

        // Remove listener
        if (exists)
            unityEvent.RemoveListener(listener);
    }

    private void RemoveAllListeners()
    {
        foreach (KeyValuePair<CustomEventType, Event> kvp in eventLibrary)
        {
            kvp.Value.RemoveAllListeners();
        }
    }

    private void RemoveAllListenersFor(CustomEventType eventType)
    {
        // Check if already existing
        Event unityEvent = null;
        bool exists = eventLibrary.TryGetValue(eventType, out unityEvent);

        unityEvent.RemoveAllListeners();
    }

    public void TriggerEvent(CustomEventType eventType, System.Object args)
    {
        // Check if already existing
        Event unityEvent = null;
        bool exists = eventLibrary.TryGetValue(eventType, out unityEvent);

        // Fire event.
        if (exists)
            unityEvent.Invoke(args);
    }

    // OnDestroy is called when the object is being destroyed
    public void OnDestroy()
    {
        if (!isInitialized)
            return;

        // Release all listeners.
        RemoveAllListeners();
    }
}
