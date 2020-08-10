using UnityEngine;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
	// UI
	private Text highscoreText = null;

	// Awake is called at initialization of this class
	void Awake()
	{
		// Start listening
		EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_UPDATE_HIGHSCORE, OnHighscoreUpdated);

		// Retrieve UI element
		highscoreText = this.gameObject.GetComponent<Text>();
	}

	// OnDestroy is called when the object is being destroyed
	void OnDestroy()
	{
		EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_UPDATE_HIGHSCORE, OnHighscoreUpdated);
	}

	private void OnHighscoreUpdated(System.Object args)
	{
		// Update current highscore.
		highscoreText.text = args.ToString();
	}
}
