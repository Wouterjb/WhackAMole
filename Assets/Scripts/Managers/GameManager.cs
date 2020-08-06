using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Singleton
    private static GameManager instance;

    // Editor variables
    [Header("Scenenames")]
    public string managerSceneName = string.Empty;
    public string startSceneName = string.Empty;
    public string uiSceneName = string.Empty;
    public string loadingSceneName = string.Empty;
    public string gameSceneName = string.Empty;

    // Collections
    private List<string> activeScenes = new List<string>();

    // String
    private string currentActiveScene = string.Empty;

    // Bool
    private bool isQuittingApplication = false;

    public static GameManager Instance
    {
        get { return instance; }
    }

    // Awake is called at the first frame
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

        activeScenes.Add(loadedScene.name);

        if (loadedScene.name.Equals(managerSceneName))
        {
            // Initialize game, this is the starting point.
            InitGame();
        }
    }

    public void OnSceneUnloaded(Scene unloadedScene)
    {
        activeScenes.Remove(unloadedScene.name);

        // Check for 0 active scenes, which means we are quitting the game
        if (activeScenes.Count == 0)
            QuitApplication();
    }

    private void InitGame()
    {
        // Load all necassary scenes, skipping game scene as that needs some more resources
        LoadScene(startSceneName);
        LoadScene(uiSceneName);
        LoadScene(loadingSceneName);
    }

    private void QuitGame()
    {
        // Unload all scenes, unloading all scenes will result in the application quitting
        for (int i = 0; i < activeScenes.Count; i++)
        {
            UnloadScene(activeScenes[i]);
        }
    }

    private void QuitApplication()
    {
        // Last chance to properly unload all data

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }
}
