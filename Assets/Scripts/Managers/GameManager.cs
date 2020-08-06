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
    private List<Scene> activeScenes = new List<Scene>();

    // String
    private string currentActiveScene = string.Empty;

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

        activeScenes.Add(loadedScene);
        SceneManager.SetActiveScene(loadedScene);

        if (loadedScene.name.Equals(managerSceneName))
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
        LoadScene(startSceneName, false);

        // Setup variables if needed..
    }

    private void QuitGame()
    {
        // Unload all scenes, unloading all scenes will result in the application quitting
        for (int i = 0; i < activeScenes.Count; i++)
        {
            UnloadScene(activeScenes[i].name);
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
}
