using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

public class GameManager : MonoBehaviour
{
    // Struct
    public struct SceneLoadOptions
    {
        public bool showLoadingScreen;
        public float minLoadingTime;
        public string nextSceneToLoad;

        // TODO: Could add a queue if you want more than one scene to be loaded at a time, but out of the scope for this assignment
    }

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
    public float minimalLoadingGameSceneTime = 0.0f;
    [Tooltip("The text shown when loading starts.")]
    public string loadingText = "Loading..";
    [Tooltip("The text shown when waiting starts.")]
    public string waitingText = "Waiting..";

    // Collections
    private List<Scene> activeScenes = new List<Scene>();

    // String
    private string currentActiveScene = string.Empty;

    // References
    private SceneLoadOptions currentSceneLoadOptions;

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
    }

    public void Start()
    {
        // Hook up scene event listeners
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    // OnDestroy is called when the object is being destroyed
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

#if UNITY_EDITOR
        Debug.Log("GameManager.OnSceneLoaded(): Current active scene: " + SceneManager.GetActiveScene().name);
#endif

        if (loadedScene.name.Equals(startSceneName))
        {
            // Initialize app, this is the starting point.
            InitApplication();
        }
        else if (loadedScene.name.Equals(uiSceneName))
        {
            // Done loading after the initial start screen, this is the point where the player has signed in, if there were such functionality.
            // TODO Retrieve player highscore from storage; event to storage manager
        }
        else if (loadedScene.name.Equals(loadingSceneName))
        {
            // Done loading loading scene, start loading async
            StartCoroutine(LoadSceneAsync(currentSceneLoadOptions));
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
        SceneManager.LoadScene(openingSceneName, LoadSceneMode.Additive);

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
        // Setup load options
        currentSceneLoadOptions.showLoadingScreen = false;
        currentSceneLoadOptions.minLoadingTime = 0.0f;
        currentSceneLoadOptions.nextSceneToLoad = uiSceneName;

        // Load the UI scene to show the start menu
        LoadSceneASync();
        UnloadScene(openingSceneName);
    }

    public void LoadSceneASync()
    {
        // Show loading screen if needed
        if (currentSceneLoadOptions.showLoadingScreen)
            SceneManager.LoadScene(loadingSceneName, LoadSceneMode.Additive);
        else
            StartCoroutine(LoadSceneAsync(currentSceneLoadOptions)); // TODO: this could be used to stream in another scene and close the current scene when done, but that is out of the scope of this assignment.
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

    private IEnumerator LoadSceneAsync(SceneLoadOptions sceneLoadOptions)
    {
        if (sceneLoadOptions.minLoadingTime > 0.0f)
        {
            float showLoadingScreenTime = 0.0f;

            // Start waiting
            EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_LOADING_STARTED, waitingText);

            // Add a little waiting time, could be used to show tips or ads by signaling the loading screen.
            while (showLoadingScreenTime < sceneLoadOptions.minLoadingTime)
            {
                showLoadingScreenTime += Time.deltaTime;

                EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_LOADING_PROGRESSED, (float)(showLoadingScreenTime / sceneLoadOptions.minLoadingTime));

                yield return new WaitForEndOfFrame();
            }
        }

        // Start loading
        EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_LOADING_STARTED, loadingText);

        // Start the operation.
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(sceneLoadOptions.nextSceneToLoad, LoadSceneMode.Additive);

        while (sceneLoadOperation.progress < 1)
        {
            // Update progress to listeners
            EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_LOADING_PROGRESSED, sceneLoadOperation.progress);

            yield return new WaitForEndOfFrame();
        }

        // Finished async loading, let all listeners know.
        EventManager.Instance.TriggerEvent(EventManager.CustomEventType.EVENT_LOADING_COMPLETED, null);

        // Clean up
        currentSceneLoadOptions.showLoadingScreen = false;
        currentSceneLoadOptions.minLoadingTime = 0.0f;
        currentSceneLoadOptions.nextSceneToLoad = string.Empty;
    }

    #region CustomEventListeners

    public void OnPlayerShowStartMenu(System.Object args)
    {
        StartUIScene();
    }

    public void OnPlayerStartsGame(System.Object args)
    {
        // Setup loading options
        currentSceneLoadOptions.showLoadingScreen = true;
        currentSceneLoadOptions.minLoadingTime = minimalLoadingGameSceneTime;
        currentSceneLoadOptions.nextSceneToLoad = gameSceneName;

        // Show the game scene
        LoadSceneASync();
    }

    #endregion
}
