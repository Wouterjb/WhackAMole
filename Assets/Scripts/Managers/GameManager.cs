using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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
        LoadScene(openingSceneName, false);

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
        // Load scenes
        LoadScene(uiSceneName, true);
        UnloadScene(openingSceneName);
    }

    public void LoadScene(string sceneName, bool aSync)
    {
        if (aSync)
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        else
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadScene(string sceneName)
    {
        // First remove from stack
        activeScenes.RemoveAt(activeScenes.Count - 1);

        // Set new scene active, there has to be an active scene before unloading
        if (activeScenes.Count > 0)
        {
            SceneManager.SetActiveScene(activeScenes[activeScenes.Count - 1]);
            Debug.Log("GameManager.UnloadScene(): Current active scene: " + SceneManager.GetActiveScene().name);
        }

        SceneManager.UnloadSceneAsync(sceneName);
    }

    #region CustomEventListeners

    public void OnPlayerShowStartMenu(System.Object args)
    {
        StartUIScene();
    }

    public void OnPlayerStartsGame(System.Object args)
    {
        
    }

    #endregion
}
