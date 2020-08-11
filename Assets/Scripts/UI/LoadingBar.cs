using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
	// Editor variables
	[Header("Loading bar references")]
	public GameObject loadingBarForeground;
	public GameObject loadingBarScreenStatusText;

	// Start is called before the first frame update
	public void Start()
	{
		EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_LOADING_STARTED, OnLoadingStarted);
		EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_LOADING_COMPLETED, OnLoadingCompleted);
		EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_LOADING_PROGRESSED, OnLoadingProgressed);
	}

	// OnDestroy is called when the object is being destroyed
	public void OnDestroy()
	{
		EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_LOADING_STARTED, OnLoadingStarted);
		EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_LOADING_COMPLETED, OnLoadingCompleted);
		EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_LOADING_PROGRESSED, OnLoadingProgressed);
	}

	private void OnLoadingStarted(System.Object args)
	{
		if (args is string)
			loadingBarScreenStatusText.GetComponent<Text>().text = (string)args;

		loadingBarForeground.GetComponent<Image>().fillAmount = 0;
	}

	private void OnLoadingCompleted(System.Object args)
	{
		loadingBarForeground.GetComponent<Image>().fillAmount = 1;
	}

	private void OnLoadingProgressed(System.Object args)
	{
		if (args is float)
			loadingBarForeground.GetComponent<Image>().fillAmount = (float)args;
	}
}
