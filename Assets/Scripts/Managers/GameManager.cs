using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager instance;

    // Editor variables
    [Header("Scenenames")]
    public string startSceneName = string.Empty;
    public string openingSceneName = string.Empty;
    public string uiSceneName = string.Empty;
    public string loadingSceneName = string.Empty;
    public string gameSceneName = string.Empty;

    [Header("Loading")]
    [Tooltip("The minimal amount of time the loading screen should be shown.")]
    public float minimalLoadingScreenTime = 0.0f;
    [Tooltip("The text shown when loading starts.")]
    public string loadingText = "Loading..";
    [Tooltip("The text shown when waiting starts.")]
    public string waitingText = "Waiting..";

    // Collections
    private List<Scene> activeScenes = new List<Scene>();

    // String
    private string currentActiveScene = string.Empty;

    public static GameManager Instance
    {
        get { return instance; }
    }

    // Awake is called at initialization of this class
    public void Awake()
    {
        // Singleton creation
        if (instance == null)
        {
            instance = GameObject.FindObjectOfType<GameManager>();
        }
        else
        {
            Destroy(this);
            return;
        }

        // Hook up scene event listeners
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    public void OnDestroy()
    {
        QuitApplication();
    }

    public void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
    {
#if UNITY_EDITOR
        Debug.Log("GameManager.OnSceneLoaded(): Loaded scene with name: " + loadedScene.name);
#endif

        // Loaded new scene, this is our active scene
        activeScenes.Add(loadedScene);
        SceneManager.SetActiveScene(loadedScene);
        Debug.Log("GameManager.OnSceneLoaded(): Current active scene: " + SceneManager.GetActiveScene().name);

        if (loadedScene.name.Equals(startSceneName))
        {
            // Initialize app, this is the starting point.
            InitApplication();
        }
    }

    public void OnSceneUnloaded(Scene unloadedScene)
    {
#if UNITY_EDITOR
        Debug.Log("GameManager.OnSceneUnloaded(): Unloaded scene with name: " + unloadedScene.name);
#endif
    }

    private void InitApplication()
    {
        // Load all necassary scenes
        LoadScene(openingSceneName, false, false);

        // Hook up custom events
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_PLAYER_SHOW_START_MENU, OnPlayerShowStartMenu);
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_PLAYER_START_GAME, OnPlayerStartsGame);

        // Setup variables if needed..
    }

    private void QuitApplication()
    {
        // Last chance to properly unload all data
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;

        // Remove custom event listeners
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_PLAYER_SHOW_START_MENU, OnPlayerShowStartMenu);
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_PLAYER_START_GAME, OnPlayerStartsGame);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void StartUIScene()
    {
        // Load the UI scene to show the start menu
        LoadScene(uiSceneName, true, false);
        UnloadScene(openingSceneName);
    }

    public void LoadScene(string sceneName, bool aSync, bool forceMinimalLoadingTime)
    {
        if (aSync)
        {
            StartCoroutine(LoadSceneAsync(sceneName, forceMinimalLoadingTime));
        }
        else
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }
    }

    public void UnloadScene(string sceneName)
    {
        // First remove from stack
        activeScenes.RemoveAt(activeScenes.Count - 1);

        // Set new scene active, there has to be an active scene before unloading
        if (activeScenes.Count > 0)
        {
            SceneManager.SetActiveScene(activeScenes[activeScenes.Count - 1]);

#if UNITY_EDITOR
            Debug.Log("GameManager.UnloadScene(): Current active scene: " + SceneManager.GetActiveScene().name);
#endif
        }

        SceneManager.UnloadSceneAsync(sceneName);
    }

    private IEnumerator LoadSceneAsync(string sceneName, bool forceMinimalLoadingTime)
    {
        if (forceMinimalLoadingTime)
        {
            float showLoadingScreenTime = 0.0f;

            // Start waiting
            EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_LOADING_STARTED, waitingText);

            // Add a little waiting time, could be used to show tips or ads by signaling the loading screen.
            while (showLoadingScreenTime < minimalLoadingScreenTime)
            {
                showLoadingScreenTime += Time.deltaTime;

                EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_LOADING_PROGRESSED, (float)(showLoadingScreenTime / minimalLoadingScreenTime));

                yield return new WaitForEndOfFrame();
            }
        }

        // Start loading
        EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_LOADING_STARTED, loadingText);

        // Start the operation.
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (sceneLoadOperation.progress < 1)
        {
            // Update progress to listeners
            EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_LOADING_PROGRESSED, sceneLoadOperation.progress);

            yield return new WaitForEndOfFrame();
        }

        // Finished async loading, let all listeners know.
        EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_LOADING_COMPLETED, null);
    }

    #region CustomEventListeners

    public void OnPlayerShowStartMenu(System.Object args)
    {
        StartUIScene();
    }

    public void OnPlayerStartsGame(System.Object args)
    {
        // Show the game scene
        LoadScene(loadingSceneName, false, false);
        LoadScene(gameSceneName, true, true);
    }

    #endregion
}
