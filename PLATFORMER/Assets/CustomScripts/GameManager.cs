using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player Manager Prefab")]
    public GameObject playerStateManagerPrefab;

    [Header("Primera Escena del Joc")]
    public string firstLevelSceneName = "Level1";

#if UNITY_EDITOR
    [Header("DEBUG MODE (Editor Only)")]
    public bool enableEditorBootstrap = true;
#endif

    private bool isTransitioning = false;

    // Variables de les dades inicials del jugador
    public float initialHealth = 100f;
    public float initialStamina = 100f;
    public int initialCoins = 0;

    private string lastLevelSceneName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("❗ GameManager duplicat trobat i destruït.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("✅ GameManager creat i persistent.");

#if UNITY_EDITOR
        if (enableEditorBootstrap)
        {
            DebugEditorSetup();
        }
#endif
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Opcionalment pots iniciar un fade in automàtic
        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeIn(1f);
        }

        EnsurePlayerStateManagerExists();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"🌍 Escena carregada: {scene.name}");

        // Guarda l'últim nivell jugat (no MainMenu/GameOver/YouWin)
        if (scene.name != "MainMenu" && scene.name != "GameOver" && scene.name != "YouWin")
        {
            lastLevelSceneName = scene.name;
            Debug.Log($"✅ Guardat últim nivell jugat: {lastLevelSceneName}");

            if (PlayerStateManager.Instance != null)
            {
                PlayerStateManager.Instance.SaveLevelStartState();
            }
        }

        if (scene.name == firstLevelSceneName)
        {
            if (PlayerStateManager.Instance == null)
            {
                Debug.Log("🧱 Instanciant PlayerStateManager...");
                Instantiate(playerStateManagerPrefab);
            }

            ResetPlayerStats();
        }
        else if (scene.name == "MainMenu")
        {
            if (PlayerStateManager.Instance != null)
            {
                Destroy(PlayerStateManager.Instance.gameObject);
                Debug.Log("🗑️ PlayerStateManager destruït");
            }
        }
    }

#if UNITY_EDITOR
    private void DebugEditorSetup()
    {
        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName != "MainMenu")
        {
            if (PlayerStateManager.Instance == null)
            {
                Debug.Log("🧪 Editor Bootstrap ➜ Instanciant PlayerStateManager manualment...");
                Instantiate(playerStateManagerPrefab);
            }

            ResetPlayerStats();
        }
    }
#endif

    private void EnsurePlayerStateManagerExists()
    {
        if (PlayerStateManager.Instance == null)
        {
            Debug.Log("🧱 Instanciant PlayerStateManager des de EnsurePlayerStateManagerExists...");
            Instantiate(playerStateManagerPrefab);

            ResetPlayerStats();
        }
    }

    #region PUBLIC METHODS

    public void StartNewGame()
    {
        Debug.Log("▶️ Nova partida: StartNewGame()");
        StartCoroutine(LoadSceneWithFade(firstLevelSceneName));
    }

    public void GoToMainMenu()
    {
        Debug.Log("🏠 Tornant al menú principal");
        StartCoroutine(LoadSceneWithFade("MainMenu"));
    }

    public void RestartLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"🔄 Reiniciant nivell actual: {currentScene}");
        StartCoroutine(LoadSceneWithFade(currentScene));
    }

    public void RestartLevelFromScratch()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"🔄 Reiniciant nivell des de zero: {currentScene}");

        ResetPlayerStats();
        StartCoroutine(LoadSceneWithFade(currentScene));
    }

    public void RestartLastLevel()
    {
        if (!string.IsNullOrEmpty(lastLevelSceneName))
        {
            Debug.Log($"🔄 Reiniciant últim nivell jugat: {lastLevelSceneName}");
            StartCoroutine(LoadSceneWithFade(lastLevelSceneName));
        }
        else
        {
            Debug.LogWarning("⚠️ No hi ha cap últim nivell guardat! Tornant al menú principal.");
            GoToMainMenu();
        }
    }

    public void StartNextLevel(string nextSceneName)
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"🚀 Passant al següent nivell: {nextSceneName}");
            StartCoroutine(LoadSceneWithFade(nextSceneName));
        }
        else
        {
            Debug.LogError("❌ No s'ha especificat el nom del següent nivell!");
        }
    }

    public void GoToGameOver()
    {
        Debug.Log("💀 Anant a l'escena GameOver");
        StartCoroutine(LoadSceneWithFade("GameOver"));
    }

    public void QuitGame()
    {
        Debug.Log("❌ Sortint del joc.");
        Application.Quit();
    }

    public void ResetPlayerStats()
    {
        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.SetPlayerState(initialHealth, initialStamina, initialCoins);
            Debug.Log($"✅ Player stats reiniciades a: Vida({initialHealth}), Estamina({initialStamina}), Monedes({initialCoins})");
        }
        else
        {
            Debug.LogWarning("❗ No s'ha pogut reiniciar el PlayerStateManager perquè no existeix.");
        }
    }

    #endregion

    #region FADE HANDLING WITH FADEMANAGER

    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        if (isTransitioning)
        {
            Debug.LogWarning("⏳ Ja hi ha una transició en procés.");
            yield break;
        }

        isTransitioning = true;

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeOut(1f);
            yield return new WaitForSeconds(1f); // Match fadeOut duration
        }

        Debug.Log($"🌐 Carregant nova escena: {sceneName}");
        SceneManager.LoadScene(sceneName);
        yield return null;

        if (FadeManager.Instance != null)
        {
            FadeManager.Instance.FadeIn(1f);
            yield return new WaitForSeconds(1f); // Match fadeIn duration
        }

        isTransitioning = false;
    }

    #endregion
}
