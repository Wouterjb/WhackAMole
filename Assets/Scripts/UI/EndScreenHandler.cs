using UnityEngine;
using UnityEngine.UI;

public class EndScreenHandler : MonoBehaviour, IUIEvent
{
    // Editor variables
    [Header("End screen texts")]
    public GameObject highScoreTextGroup = null;
    public GameObject highScorePointsText = null;

    [Header("End screen button")]
    public GameObject endScreenButton = null;
    [Tooltip("The amount of time between showing the end screen and the end button, to prevent clickthrough")]
    public float showEndScreenButtonAfter = 0.5f;

    // Numbers
    private float currentShowEndScreenBtnTimer = 0.0f;

    // OnDestroy is called when the object is being destroyed
    public void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_NEW_HIGHSCORE, OnNewHighScore);
    }

    // This function is called when the object becomes disabled or inactive
    public void OnDisable()
    {
        highScoreTextGroup.SetActive(false);
        endScreenButton.SetActive(false);
    }

    // Update is called once per frame
    public void Update()
    {
        if (!endScreenButton.activeSelf)
        {
            // Stagger showing the end game button to prevent click through after completing the game.
            currentShowEndScreenBtnTimer += Time.deltaTime;

            if (currentShowEndScreenBtnTimer >= showEndScreenButtonAfter)
            {
                endScreenButton.SetActive(true);
                currentShowEndScreenBtnTimer = 0.0f;
            }
        }
    }

    private void OnNewHighScore(System.Object args)
    {
        highScoreTextGroup.SetActive(true);
        highScorePointsText.GetComponent<Text>().text = args.ToString();
    }

    public void InitEventListener()
    {
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_NEW_HIGHSCORE, OnNewHighScore);
    }
}
