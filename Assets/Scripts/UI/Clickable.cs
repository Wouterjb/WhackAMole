using UnityEngine;

public class Clickable : MonoBehaviour
{
    // Editor variables
    [Header("Event type")]
    public EventManager.CustomEventType eventToTrigger;
    public GameObject argument;

    public void OnClick()
    {
        switch (eventToTrigger)
        {
            case EventManager.CustomEventType.EVENT_ACTIVATE_CANVAS:
                EventManager.Instance.TriggerEvent(eventToTrigger, argument);
                break;

            default:
                EventManager.Instance.TriggerEvent(eventToTrigger, null);
                break;
        }
    }
}
