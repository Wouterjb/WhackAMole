using UnityEngine.UI;
using UnityEngine;

public class Highscore : MonoBehaviour
{
	// UI
	private Text highscoreText = null;

	// Awake is called at initialization of this class
	public void Awake()
	{
		// Retrieve UI element
		highscoreText = this.gameObject.GetComponent<Text>();
		highscoreText.text = StorageManager.Instance.playerHighestScore.ToString();
	}

	// This function is called when the object becomes enabled or active
	public void OnEnable()
	{
		highscoreText.text = StorageManager.Instance.playerHighestScore.ToString();
	}
}
