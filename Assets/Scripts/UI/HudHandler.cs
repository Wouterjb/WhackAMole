using System;
using UnityEngine;
using UnityEngine.UI;

public class HudHandler : MonoBehaviour
{
    // Editor variables
    [Header("Hud texts")]
    public GameObject scoreText = null;
    public GameObject timeText = null;

    // Start is called before the first frame update
    public void Start()
    {
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_TOTAL_SCORED_POINTS, OnTotalScoredPoints);
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_UPDATE_SESSION_TIME, OnTimeLeftUpdate);
    }

    // OnDestroy is called when the object is being destroyed
    public void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_TOTAL_SCORED_POINTS, OnTotalScoredPoints);
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_UPDATE_SESSION_TIME, OnTimeLeftUpdate);
    }

    // This function is called when the object becomes enabled
    public void OnEnable()
    {
        scoreText.GetComponent<Text>().text = "0";
        timeText.GetComponent<Text>().text = string.Empty;
    }

    private void OnTotalScoredPoints(System.Object args)
    {
        scoreText.GetComponent<Text>().text = args.ToString();
    }

    private void OnTimeLeftUpdate(System.Object args)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds((float)args);

        timeText.GetComponent<Text>().text = timeSpan.ToString("%s") + " sec";
    }
}
