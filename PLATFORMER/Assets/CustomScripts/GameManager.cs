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

    // Variables de les dades inicials del jugador
    public float initialHealth = 100f;
    public float initialStamina = 100f;
    public int initialCoins = 0;

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
        SceneManager.sceneLoaded += OnSceneLoaded;

        if (fadeImage != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == firstLevelSceneName)
        {
            if (PlayerStateManager.Instance == null)
            {
                Instantiate(playerStateManagerPrefab);
            }

            ResetPlayerStats(); // 👈 Reiniciem les estadístiques sempre que es carregui el nivell!

            FindFadeImage();
        }
        else if (scene.name == "MainMenu")
        {
            if (PlayerStateManager.Instance != null)
            {
                Destroy(PlayerStateManager.Instance.gameObject);
            }

            FindFadeImage();
        }
    }

    #region PUBLIC METHODS

    public void StartNewGame()
    {
        Debug.Log("▶️ Nova partida");
        StartCoroutine(LoadSceneWithFade(firstLevelSceneName));
    }

    public void GoToMainMenu()
    {
        Debug.Log("🏠 Tornant al menú principal");
        StartCoroutine(LoadSceneWithFade("MainMenu"));
    }

    public void RestartLevel()
    {
        Debug.Log("🔄 Reiniciant nivell");
        StartCoroutine(LoadSceneWithFade(SceneManager.GetActiveScene().name));
    }

    public void QuitGame()
    {
        Debug.Log("❌ Sortint del joc");
        Application.Quit();
    }

    public void ResetPlayerStats()
    {
        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.SetPlayerState(initialHealth, initialStamina, initialCoins);
            Debug.Log("✅ Player stats reiniciades!");
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
            yield break;

        isTransitioning = true;

        if (fadeImage != null)
            yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene(sceneName);
        yield return null;

        FindFadeImage();

        if (fadeImage != null)
            yield return StartCoroutine(FadeIn());

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
    }

    private void FindFadeImage()
    {
        if (fadeImage == null)
        {
            GameObject fadeObj = GameObject.Find("FadeImage");

            if (fadeObj != null)
            {
                fadeImage = fadeObj.GetComponent<Image>();
                Debug.Log("FadeImage trobat i assignat.");
            }
            else
            {
                Debug.LogWarning("❗ FadeImage no trobat a la nova escena.");
            }
        }
    }

    #endregion
}
