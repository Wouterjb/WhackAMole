using UnityEngine;

/// <summary>
/// Could use this class for showing ads, tips while loading
/// </summary>
public class LoadingScreenHandler : MonoBehaviour
{
	// Start is called before the first frame update
	public void Start()
	{
		EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_LOADING_COMPLETED, OnLoadingCompleted);
	}

	// OnDestroy is called when the object is being destroyed
	public void OnDestroy()
	{
		EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_LOADING_COMPLETED, OnLoadingCompleted);
	}

	private void OnLoadingCompleted(System.Object args)
	{
		this.gameObject.SetActive(false);
	}
}
