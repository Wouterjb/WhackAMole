using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
	// Editor variables
	[Header("Loading bar references")]
	public GameObject loadingBarForeground;
	public GameObject loadingBarScreenStatusText;

	// Awake is called at initialization of this class
	public void Awake()
	{
		EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_LOADING_STARTED, OnLoadingStarted);
		EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_LOADING_COMPLETED, OnLoadingCompleted);
		EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_LOADING_PROGRESSED, OnLoadingProgressed);
	}

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
