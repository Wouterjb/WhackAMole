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

    public void OnSceneLoaded(Scene loadedScene, LoadSceneMode loadSceneMode)
    {
#if UNITY_EDITOR
        Debug.Log("GameManager.OnSceneLoaded(): Loaded scene with name: " + loadedScene.name);
#endif

        activeScenes.Add(loadedScene);
        SceneManager.SetActiveScene(loadedScene);

        if (loadedScene.name.Equals(startSceneName))
        {
            // Initialize game, this is the starting point.
            InitGame();
        }
    }

    public void OnSceneUnloaded(Scene unloadedScene)
    {
#if UNITY_EDITOR
        Debug.Log("GameManager.OnSceneUnloaded(): Unloaded scene with name: " + unloadedScene.name);
#endif

        activeScenes.Remove(unloadedScene);
        SceneManager.SetActiveScene(activeScenes[activeScenes.Count - 1]);

        // Check for 0 active scenes, which means we are quitting the game
        if (activeScenes.Count == 0)
            QuitApplication();
    }

    private void InitGame()
    {
        // Load all necassary scenes
        LoadScene(openingSceneName, false);

        // Hook up custom events
        EventManager.Instance.AddListener(EventManager.CustomEventType.EVENT_PLAYER_START_GAME, OnPlayerStartsGame);

        // Setup variables if needed..
    }

    private void QuitApplication()
    {
        // Last chance to properly unload all data
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        EventManager.Instance.RemoveListener(EventManager.CustomEventType.EVENT_PLAYER_START_GAME, OnPlayerStartsGame);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void StartGame()
    {
        // Load scenes
        LoadScene(uiSceneName, true);
        UnloadScene(openingSceneName);
    }

    private void QuitGame()
    {
        // Unload all scenes, unloading all scenes will result in the application quitting
        for (int i = 0; i < activeScenes.Count; i++)
        {
            UnloadScene(activeScenes[i].name);
        }
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
        SceneManager.UnloadSceneAsync(sceneName);
    }

    #region CustomEventListeners

    public void OnPlayerStartsGame(System.Object args)
    {
        StartGame();
    }

    #endregion
}
