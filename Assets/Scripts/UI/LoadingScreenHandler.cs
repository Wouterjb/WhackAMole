using UnityEngine;

/// <summary>
/// Could use this class for showing ads, tips while loading
/// </summary>
public class LoadingScreenHandler : MonoBehaviour
{
	// Awake is called at initialization of this class
	public void Awake()
	{
		EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_LOADING_COMPLETED, OnLoadingCompleted);
	}

	public void OnDestroy()
	{
		EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_LOADING_COMPLETED, OnLoadingCompleted);
	}

	private void OnLoadingCompleted(System.Object args)
	{
		this.gameObject.SetActive(false);
	}
}
