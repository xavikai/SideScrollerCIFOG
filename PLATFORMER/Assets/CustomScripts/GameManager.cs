using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estat del Jugador")]
    public float playerHealth = 100f;
    public float playerStamina = 100f;
    public int playerCoins = 0;

    [Header("Fade Control")]
    public Image fadeImage;              // Arrossega el FadeImage des del Canvas de l'escena inicial (MainMenu)
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    [Header("Nom de l'escena inicial")]
    public string firstLevelSceneName = "Level1";    // Nom de l'escena del primer nivell

    private void Awake()
    {
        // Singleton Pattern ➜ assegura que només hi hagi un GameManager persistent
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Inicia FadeIn si es troba la imatge
        if (fadeImage != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    #region Funcions d'estat del jugador

    public void ResetPlayerStats()
    {
        playerHealth = 100f;
        playerStamina = 100f;
        playerCoins = 0;

        Debug.Log("✅ Estadístiques del jugador reiniciades!");
    }

    public void SavePlayerState(float health, float stamina, int coins)
    {
        playerHealth = health;
        playerStamina = stamina;
        playerCoins = coins;

        Debug.Log($"💾 Estat del jugador guardat ➜ Vida: {playerHealth}, Estamina: {playerStamina}, Monedes: {playerCoins}");
    }

    public void LoadPlayerState(PlayerStateManager playerState)
    {
        if (playerState == null)
        {
            Debug.LogWarning("⚠️ No s'ha trobat el PlayerStateManager per carregar estat!");
            return;
        }

        playerState.currentHealth = playerHealth;
        playerState.currentStamina = playerStamina;
        playerState.currentCoins = playerCoins;

        Debug.Log($"📥 Estat del jugador carregat ➜ Vida: {playerHealth}, Estamina: {playerStamina}, Monedes: {playerCoins}");
    }

    #endregion

    #region Funcions de joc i escenes

    public void StartNewGame()
    {
        ResetPlayerStats();

        if (!string.IsNullOrEmpty(firstLevelSceneName))
        {
            LoadScene(firstLevelSceneName);
        }
        else
        {
            Debug.LogError("❌ No s'ha especificat el nom de l'escena inicial!");
        }
    }

    public void RestartLevel()
    {
        ResetPlayerStats();

        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log("🔄 Reiniciant nivell actual: " + currentScene);

        LoadScene(currentScene);
    }

    public void GoToMainMenu()
    {
        Debug.Log("🏠 Tornant al Main Menu...");

        ResetPlayerStats();

        LoadScene("MainMenu");
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneWithFade(sceneName));
    }

    public void QuitGame()
    {
        Debug.Log("🚪 Sortint del joc...");
        Application.Quit();
    }

    #endregion

    #region Gestió de canvi d'escena amb Fade

    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        Debug.Log($"🔀 Carregant escena: {sceneName}");

        // FadeOut abans de carregar
        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeOut());
        }

        SceneManager.LoadScene(sceneName);
        yield return null;

        // Buscar de nou el FadeImage a la nova escena
        FindFadeImage();

        // FadeIn després de carregar
        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeIn());
        }
    }

    private void FindFadeImage()
    {
        if (fadeImage == null)
        {
            GameObject fadeObj = GameObject.Find("FadeImage");

            if (fadeObj != null)
            {
                fadeImage = fadeObj.GetComponent<Image>();
                Debug.Log("✅ FadeImage trobat i assignat automàticament.");
            }
            else
            {
                Debug.LogWarning("⚠️ FadeImage no trobat a l'escena nova!");
            }
        }
    }

    #endregion

    #region Fade In / Out

    private IEnumerator FadeIn()
    {
        if (fadeImage == null)
        {
            Debug.LogWarning("⚠️ No hi ha FadeImage per fer FadeIn!");
            yield break;
        }

        float elapsed = 0f;
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            c.a = 1 - Mathf.Clamp01(elapsed / fadeInDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 0f;
        fadeImage.color = c;
    }

    private IEnumerator FadeOut()
    {
        if (fadeImage == null)
        {
            Debug.LogWarning("⚠️ No hi ha FadeImage per fer FadeOut!");
            yield break;
        }

        float elapsed = 0f;
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsed / fadeOutDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;
    }

    #endregion
}
