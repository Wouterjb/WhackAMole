using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
	// UI
	private Text highscoreText = null;

	// Awake is called at initialization of this class
	public void Awake()
	{
		// Retrieve UI element
		highscoreText = this.gameObject.GetComponent<Text>();
	}

	// Start is called before the first frame update
	public void Start()
	{
		// Start listening
		EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_NEW_HIGHSCORE, OnHighscoreUpdated);
	}

	// OnDestroy is called when the object is being destroyed
	public void OnDestroy()
	{
		EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_NEW_HIGHSCORE, OnHighscoreUpdated);
	}

	private void OnHighscoreUpdated(System.Object args)
	{
		// Update current highscore.
		highscoreText.text = args.ToString();
	}
}
