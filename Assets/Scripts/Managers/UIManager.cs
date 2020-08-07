using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Singleton
    private static UIManager instance;

    // Editorvariables
    [Header("Canvas objects")]
    public GameObject startMenuCanvas = null;
    public GameObject optionsMenuCanvas = null;
    public GameObject hudCanvas = null;

    [Header("Initialization")]
    public GameObject firstActiveCanvas = null;

    // References
    private GameObject currentActiveCanvas = null;

    public static UIManager Instance
    {
        get { return instance; }
    }

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

        // Subscribe to custom events
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_ACTIVATE_CANVAS, OnActivateCanvas);
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_PLAYER_START_GAME, OnPlayerStartedGame);

        // Set initial canvas active
        ActivateCanvas(firstActiveCanvas);
    }

    public void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_ACTIVATE_CANVAS, OnActivateCanvas);
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_PLAYER_START_GAME, OnPlayerStartedGame);
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

    private void OnPlayerStartedGame(System.Object args)
    {
        DeActivateCanvas(currentActiveCanvas);
    }
}
