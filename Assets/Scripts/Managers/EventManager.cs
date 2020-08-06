using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public enum CustomEventType
    {
        EVENT_NONE = 0,
        EVENT_PLAYER_START_GAME = 1,
    }

    // Singleton
    private static EventManager instance;

    public static EventManager Instance
    {
        get { return instance; }
    }

    // Collections
    private Dictionary<CustomEventType, UnityEvent> eventLibrary;

    // Awake is called at initialization of this class
    void Awake()
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

        eventLibrary = new Dictionary<CustomEventType, UnityEvent>();
    }

    public void AddListener(CustomEventType eventType, UnityAction listener)
    {
        // Check if already existing
        UnityEvent unityEvent = null;
        bool exists = eventLibrary.TryGetValue(eventType, out unityEvent);

        if (exists)
        {
            // Already exists, add listener to event
            unityEvent.AddListener(listener);
        } 
        else
        {
            // Create a new one
            unityEvent = new UnityEvent();
            unityEvent.AddListener(listener);
            eventLibrary.Add(eventType, unityEvent);
        }
    }

    public void RemoveListener(CustomEventType eventType, UnityAction listener)
    {
        // Check if already existing
        UnityEvent unityEvent = null;
        bool exists = eventLibrary.TryGetValue(eventType, out unityEvent);

        // Remove listener
        if (exists)
            unityEvent.RemoveListener(listener);
    }

    public void RemoveAllListeners()
    {
        foreach (KeyValuePair<CustomEventType, UnityEvent> kvp in eventLibrary)
        {
            kvp.Value.RemoveAllListeners();
        }
    }

    public void RemoveAllListenersFor(CustomEventType eventType)
    {
        // Check if already existing
        UnityEvent unityEvent = null;
        bool exists = eventLibrary.TryGetValue(eventType, out unityEvent);

        unityEvent.RemoveAllListeners();
    }

    public void TriggerEvent(CustomEventType eventType)
    {
        // Check if already existing
        UnityEvent unityEvent = null;
        bool exists = eventLibrary.TryGetValue(eventType, out unityEvent);

        // Fire event.
        if (exists)
            unityEvent.Invoke();
    }
}
