using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton
    private static UIManager instance;

    // Editor variables
    [Header("User interface references")]
    public GameObject startScreen;
    public GameObject playerHud;

    // References
    private GameObject currentActiveCanvas = null;

    public static UIManager Instance
    {
        get { return instance; }
    }

    // Awake is called at initialization of this class
    public void Awake()
    {
        // Singleton creation
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<UIManager>();
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    // Start is called before the first frame update
    public void Start()
    {
        // Subscribe to custom events
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_ACTIVATE_CANVAS, OnActivateCanvas);
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_PLAYER_START, OnPlayerStart);
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_SESSION_START, OnSessionReady);

        ActivateCanvas(startScreen);
    }

    // OnDestroy is called when the object is being destroyed
    public void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_ACTIVATE_CANVAS, OnActivateCanvas);
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_PLAYER_START, OnPlayerStart);
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_SESSION_START, OnSessionReady);
    }

    private void ActivateCanvas(GameObject canvas)
    {
        if (canvas == null)
        {
#if UNITY_EDITOR
            throw new System.Exception("Canvas is null");
#else
            return;
#endif
        }

        // First deactivate the current canvas
        DeActivateCanvas(currentActiveCanvas);

        // Activate new one
        canvas.SetActive(true);
        currentActiveCanvas = canvas;
    }

    private void DeActivateCanvas(GameObject canvas)
    {
        if (canvas != null)
            canvas.SetActive(false);
    }

    private void OnActivateCanvas(System.Object args)
    {
        if (args is GameObject)
            ActivateCanvas((GameObject)args);
    }

    private void OnPlayerStart(System.Object args)
    {
        DeActivateCanvas(currentActiveCanvas);
    }

    private void OnSessionReady(System.Object args)
    {
        ActivateCanvas(playerHud);
    }
}
