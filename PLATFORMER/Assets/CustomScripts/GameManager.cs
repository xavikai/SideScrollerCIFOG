using UnityEngine;
using UnityEngine.SceneManagement;   // Per controlar escenes
using UnityEngine.UI;                // Per accedir a UI Image
using System.Collections;            // Per IEnumerator

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("FADE")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Player Manager Prefab")]
    public GameObject playerStateManagerPrefab;

    [Header("Primera Escena del Joc")]
    public string firstLevelSceneName = "Level1";

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
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (fadeImage != null)
        {
            Debug.Log("🎬 Iniciant fade in...");
            StartCoroutine(FadeIn());
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"🌍 Escena carregada: {scene.name}");

        // Si no és menú ni finals, guarda l'últim nivell jugat
        if (scene.name != "MainMenu" && scene.name != "GameOver" && scene.name != "YouWin")
        {
            lastLevelSceneName = scene.name;
            Debug.Log($"✅ Guardat últim nivell jugat: {lastLevelSceneName}");

            if (PlayerStateManager.Instance != null)
            {
                PlayerStateManager.Instance.SaveLevelStartState(); // Guarda l'estat del jugador al començament del nivell
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
            FindFadeImage();
        }
        else if (scene.name == "MainMenu")
        {
            if (PlayerStateManager.Instance != null)
            {
                Destroy(PlayerStateManager.Instance.gameObject);
                Debug.Log("🗑️ PlayerStateManager destruït");
            }

            FindFadeImage();
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

    // 👉 NOU MÈTODE que reinicia el nivell des de zero
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

    #region FADE METHODS

    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        if (isTransitioning)
        {
            Debug.LogWarning("⏳ Ja hi ha una transició en procés.");
            yield break;
        }

        isTransitioning = true;

        if (fadeImage != null)
        {
            Debug.Log("🎬 Iniciant FadeOut...");
            yield return StartCoroutine(FadeOut());
        }

        Debug.Log($"🌐 Carregant nova escena: {sceneName}");
        SceneManager.LoadScene(sceneName);
        yield return null;

        FindFadeImage();

        if (fadeImage != null)
        {
            Debug.Log("🎬 Iniciant FadeIn...");
            yield return StartCoroutine(FadeIn());
        }

        // Restaura estat del jugador després de carregar escena (no MainMenu)
        if (sceneName != "MainMenu" && PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.RestoreLevelStartState();
        }

        isTransitioning = false;
    }

    private IEnumerator FadeIn()
    {
        if (fadeImage == null) yield break;

        float elapsedTime = 0f;
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = 1f - Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;

        Debug.Log("✅ FadeIn complet.");
    }

    private IEnumerator FadeOut()
    {
        if (fadeImage == null) yield break;

        float elapsedTime = 0f;
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;

        Debug.Log("✅ FadeOut complet.");
    }

    private void FindFadeImage()
    {
        if (fadeImage == null)
        {
            GameObject fadeObj = GameObject.Find("FadeImage");

            if (fadeObj != null)
            {
                fadeImage = fadeObj.GetComponent<Image>();
                Debug.Log("🔎 FadeImage trobat i assignat.");
            }
            else
            {
                Debug.LogWarning("⚠️ FadeImage no trobat a la nova escena.");
            }
        }
        else
        {
            Debug.Log("ℹ️ fadeImage ja està assignat.");
        }
    }

    #endregion
}
