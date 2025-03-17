using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using StarterAssets;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Estat del Jugador")]
    public float playerHealth = 100f;
    public float playerStamina = 100f;
    public int playerCoins = 0;

    [Header("Control del Fade")]
    public Image fadeImage;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    [Header("Escenes del Joc")]
#if UNITY_EDITOR
    [Tooltip("Escena inicial del joc. Arrossega aquí la escena!")]
    public SceneAsset firstLevelScene;
#endif
    [HideInInspector]
    public string firstLevelSceneName;

    private void Awake()
    {
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
        if (fadeImage != null)
        {
            StartCoroutine(FadeIn());
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (firstLevelScene != null)
        {
            firstLevelSceneName = firstLevelScene.name;
            EditorUtility.SetDirty(this);
        }
    }
#endif

    #region Funcions Estat del Jugador

    public void ResetPlayerStats()
    {
        playerHealth = 100f;
        playerStamina = 100f;
        playerCoins = 0;

        Debug.Log("Estat del jugador reiniciat a valors inicials!");
    }

    public void SavePlayerState(float health, float stamina, int coins)
    {
        playerHealth = health;
        playerStamina = stamina;
        playerCoins = coins;

        Debug.Log($"Estat guardat -> Vida: {playerHealth}, Estamina: {playerStamina}, Monedes: {playerCoins}");
    }

    public void LoadPlayerState(PlayerUIController controller)
    {
        if (controller == null)
        {
            Debug.LogWarning("No s'ha proporcionat cap PlayerUIController per carregar estat!");
            return;
        }

        controller.SetValues(playerHealth, playerStamina, playerCoins);

        Debug.Log($"Estat carregat al PlayerUIController -> Vida: {playerHealth}, Estamina: {playerStamina}, Monedes: {playerCoins}");
    }

    #endregion

    #region Funcions de Joc

    public void StartNewGame()
    {
        ResetPlayerStats();

        if (!string.IsNullOrEmpty(firstLevelSceneName))
        {
            StartCoroutine(LoadSceneWithFade(firstLevelSceneName));
        }
        else
        {
            Debug.LogError("No s'ha assignat cap escena inicial al GameManager!");
        }
    }

    public void RestartLevel()
    {
        ResetPlayerStats();

        string currentSceneName = SceneManager.GetActiveScene().name;
        Debug.Log($"Reiniciant nivell actual: {currentSceneName}");

        StartCoroutine(LoadSceneWithFade(currentSceneName));
    }

    public void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("No s'ha proporcionat cap nom d'escena!");
            return;
        }

        StartCoroutine(LoadSceneWithFade(sceneName));
    }

    public void QuitGame()
    {
        Debug.Log("Sortint del joc...");
        Application.Quit();
    }

    #endregion

    #region Gestió de Canvi d'Escena amb Fade

    private IEnumerator LoadSceneWithFade(string sceneName)
    {
        Debug.Log($"Carregant escena: {sceneName}");

        if (fadeImage != null)
        {
            yield return StartCoroutine(FadeOut());
        }

        SceneManager.LoadScene(sceneName);
        yield return null;

        FindFadeImage();

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
                Debug.Log("FadeImage trobat i assignat automàticament.");
            }
            else
            {
                Debug.LogWarning("FadeImage no trobat a l'escena nova! Assegura't que hi és.");
            }
        }
    }

    #endregion

    #region Fade In / Out

    private IEnumerator FadeIn()
    {
        if (fadeImage == null)
        {
            Debug.LogWarning("FadeImage no assignada al FadeIn!");
            yield break;
        }

        float elapsedTime = 0f;
        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = 1 - Mathf.Clamp01(elapsedTime / fadeInDuration);
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
            Debug.LogWarning("FadeImage no assignada al FadeOut!");
            yield break;
        }

        float elapsedTime = 0f;
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / fadeOutDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = 1f;
        fadeImage.color = c;
    }

    #endregion
}
