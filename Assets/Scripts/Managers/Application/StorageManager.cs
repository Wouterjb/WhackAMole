using UnityEngine;

public class StorageManager : MonoBehaviour
{
    // Constants
    private const string PLAYER_HIGH_SCORE = "PlayerHighScore";

    // Singleton
    private static StorageManager instance;

    // Numbers
    [HideInInspector]
    public int playerHighestScore = 0;

    public static StorageManager Instance
    {
        get { return instance; }
    }

    // Awake is called at initialization of this class
    public void Awake()
    {
        // Singleton creation
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<StorageManager>();
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
        // Listen for events
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_NEW_HIGHSCORE, OnNewHighScore);
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_INIT_GAME, OnInitStorage);
    }


    // OnDestroy is called when the object is being destroyed
    public void OnDestroy()
    {
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_NEW_HIGHSCORE, OnNewHighScore);
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_INIT_GAME, OnInitStorage);
    }

    private void InitStorage()
    {
        // Highscore
        bool succes = RetrieveValue(PLAYER_HIGH_SCORE, 0, out playerHighestScore);

        if (!succes)
            PlayerPrefs.SetInt(PLAYER_HIGH_SCORE, playerHighestScore);

        // Done initializing, let everyone know
        EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_STORAGE_INTIALIZED, null);
    }

    private void ClearStorage()
    {
#if UNITY_EDITOR
        // Debug functionality!
        PlayerPrefs.DeleteAll();
        ResetStorage();
#endif
    }

    private void ResetStorage()
    {
        playerHighestScore = 0;
    }

    private bool RetrieveValue(string key, int initialValue, out int value)
    {
        // Check if there is already an entry
        bool hasStorageKey = PlayerPrefs.HasKey(key);

        if (hasStorageKey)
        {
            // Found entry, retrieve it
            value = PlayerPrefs.GetInt(key);
        }
        else
        {
            // No entry found, set initial value
            value = initialValue;
        }

        return hasStorageKey;
    }

    private void OnNewHighScore(System.Object args)
    {
        // New high score! Let's store it.
        playerHighestScore = (int)args;

        PlayerPrefs.SetInt(PLAYER_HIGH_SCORE, playerHighestScore);

        // Save storage
        PlayerPrefs.Save();
    }

    private void OnInitStorage(System.Object args)
    {
        InitStorage();
    }
}
