using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameOverController : MonoBehaviour
{
    [Header("Botons")]
    public Button restartButton;
    public Button mainMenuButton;

    [Header("Assigna les escenes arrossegant-les aquí (Editor només)")]
#if UNITY_EDITOR
    public SceneAsset gameScene;
    public SceneAsset mainMenuScene;
#endif

    [Header("Noms de les escenes (es posen sols, no toquis!)")]
    public string gameSceneName;
    public string mainMenuSceneName;

    [Header("Música de l'escena")]
    [Tooltip("Arrossega aquí el clip de música per la pantalla de Game Over")]
    public AudioClip musicClip;

    [Range(0f, 1f)]
    [Tooltip("Volum de la música de Game Over")]
    public float musicVolume = 0.5f;

    private AudioSource musicSource;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (gameScene != null)
        {
            gameSceneName = gameScene.name;
            EditorUtility.SetDirty(this);
        }

        if (mainMenuScene != null)
        {
            mainMenuSceneName = mainMenuScene.name;
            EditorUtility.SetDirty(this);
        }
    }
#endif

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        else
            Debug.LogWarning("No s'ha assignat el botó de restart.");

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        else
            Debug.LogWarning("No s'ha assignat el botó de menú principal.");

        SetupMusic();
    }

    private void SetupMusic()
    {
        if (musicClip != null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = musicClip;
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.volume = musicVolume;
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("No s'ha assignat cap música a l'escena Game Over!");
        }
    }

    public void RestartGame()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("No s'ha assignat cap escena de joc!");
        }
    }

    public void GoToMainMenu()
    {
        if (!string.IsNullOrEmpty(mainMenuSceneName))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("No s'ha assignat cap escena de menú principal!");
        }
    }
}
