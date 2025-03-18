using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

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

    [Header("Valors inicials del jugador")]
    public float initialHealth = 100f;
    public float initialStamina = 100f;
    public int initialCoins = 0;

    private void Awake()
    {
        // Singleton pattern per assegurar un únic GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("✅ GameManager creat i persistent.");
        }
        else
        {
            Debug.LogWarning("❗ GameManager duplicat trobat i destruït.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (fadeImage != null)
        {
            Debug.Log("🎬 Iniciant fade in...");
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogWarning("⚠️ No s'ha assignat el fadeImage al GameManager!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"🌍 Escena carregada: {scene.name}");

        if (scene.name == firstLevelSceneName)
        {
            if (PlayerStateManager.Instance == null)
            {
                Debug.Log("🧱 Instanciant PlayerStateManager...");
                Instantiate(playerStateManagerPrefab);
            }

            ResetPlayerStats(); // Reinicia les dades del jugador cada cop que tornem al primer nivell
            FindFadeImage();
        }
        else if (scene.name == "MainMenu")
        {
            Debug.Log("🏠 Tornant al MainMenu.");

            if (PlayerStateManager.Instance != null)
            {
                Debug.Log("🗑️ Destruint PlayerStateManager existent.");
                Destroy(PlayerStateManager.Instance.gameObject);
            }

            FindFadeImage();
        }
    }

    #region PUBLIC METHODS

    public void StartNewGame()
    {
        Debug.Log("▶️ Nova partida: StartNewGame() cridat.");

        if (string.IsNullOrEmpty(firstLevelSceneName))
        {
            Debug.LogError("❌ No s'ha assignat el nom de la primera escena!");
            return;
        }

        StartCoroutine(LoadSceneWithFade(firstLevelSceneName));
    }

    public void GoToMainMenu()
    {
        Debug.Log("🏠 Tornant al menú principal: GoToMainMenu() cridat.");
        StartCoroutine(LoadSceneWithFade("MainMenu"));
    }

    public void RestartLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"🔄 Reiniciant nivell actual: {currentScene}");

        StartCoroutine(LoadSceneWithFade(currentScene));
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

        isTransitioning = false;
    }

    private IEnumerator FadeIn()
    {
        if (fadeImage == null)
        {
            Debug.LogWarning("⚠️ FadeIn cancel·lat. fadeImage no assignat.");
            yield break;
        }

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
        if (fadeImage == null)
        {
            Debug.LogWarning("⚠️ FadeOut cancel·lat. fadeImage no assignat.");
            yield break;
        }

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
