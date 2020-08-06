using UnityEngine;

public class Clickable : MonoBehaviour
{
    // Editor variables
    [Header("Event type")]
    public EventManager.CustomEventType eventToTrigger;

    public void OnClick()
    {
        EventManager.Instance.TriggerEvent(eventToTrigger);
    }
}
