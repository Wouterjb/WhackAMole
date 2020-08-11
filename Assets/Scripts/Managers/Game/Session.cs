using UnityEngine;

public class Session : MonoBehaviour
{
    // Editor variables
    [Header("Session parameters")]
    [Tooltip("The amount of time per session in seconds")]
    public float sessionTime = 30.0f;

    // Numbers
    private int totalScore = 0;
    private float currentSessionTime = 0.0f;

    // Bool
    private bool sessionEnded = false;

    // Start is called before the first frame update
    public void Start()
    {
        EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_SESSION_START, null);
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_SCORED_POINTS, OnScoredPoints);
    }

    // OnDestroy is called when the object is being destroyed
    public void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_SCORED_POINTS, OnScoredPoints);
    }

    // Update is called once per frame
    public void Update()
    {
        if (!sessionEnded)
        {
            currentSessionTime += Time.deltaTime;

            if (currentSessionTime >= sessionTime)
            {
                sessionEnded = true;

                // End game.
                EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_TOTAL_SCORED_POINTS, totalScore);
                EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_SESSION_END, null);
                EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_UPDATE_SESSION_TIME, 0.0f);
            }
            else
            {
                EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_UPDATE_SESSION_TIME, sessionTime - currentSessionTime);
            }
        }
    }

    private void OnScoredPoints(System.Object args)
    {
        totalScore += (int)args;
        EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_TOTAL_SCORED_POINTS, totalScore);
    }
}
