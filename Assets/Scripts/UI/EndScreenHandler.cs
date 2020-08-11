using UnityEngine;
using UnityEngine.UI;

public class EndScreenHandler : MonoBehaviour
{
    // Editor variables
    [Header("End screen texts")]
    public GameObject newHighScoreText = null;
    public GameObject newHighScorePointsText = null;

    // Start is called before the first frame update
    public void Start()
    {
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_NEW_HIGHSCORE, OnNewHighScore);
    }

    // OnDestroy is called when the object is being destroyed
    public void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_NEW_HIGHSCORE, OnNewHighScore);
    }

    private void OnNewHighScore(System.Object args)
    {
        newHighScoreText.SetActive(true);
        newHighScorePointsText.SetActive(true);

        newHighScorePointsText.GetComponent<Text>().text = args.ToString();
    }
}
