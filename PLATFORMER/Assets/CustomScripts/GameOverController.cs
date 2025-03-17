using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameOverController : MonoBehaviour
{
    [Header("Botons")]
    [Tooltip("Bot� per reiniciar el joc")]
    public Button restartButton;

    [Tooltip("Bot� per tornar al men� principal")]
    public Button mainMenuButton;

    [Header("Assigna les escenes arrossegant-les aqu� (Editor nom�s)")]
#if UNITY_EDITOR
    public SceneAsset gameScene;         // Escena del joc
    public SceneAsset mainMenuScene;     // Escena del men� principal
#endif

    [Header("Noms de les escenes (es posen sols, no toquis!)")]
    public string gameSceneName;         // Es fa servir en runtime
    public string mainMenuSceneName;

#if UNITY_EDITOR
    // Aquesta funci� es crida autom�ticament quan canvies alguna cosa a l'inspector
    private void OnValidate()
    {
        if (gameScene != null)
        {
            gameSceneName = gameScene.name;
            EditorUtility.SetDirty(this); // Guarda el canvi a l'inspector
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
        // Mostrar i desbloquejar el cursor (imprescindible per UI)
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Assigna els listeners als botons (millor aqu� que des de l'inspector)
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        else
            Debug.LogWarning("No s'ha assignat el bot� de restart.");

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        else
            Debug.LogWarning("No s'ha assignat el bot� de men� principal.");
    }

    public void RestartGame()
    {
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            // Bloqueja el cursor si el teu joc ho necessita
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
            // Mostrem el cursor al men� per si de cas
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            SceneManager.LoadScene(mainMenuSceneName);
        }
        else
        {
            Debug.LogError("No s'ha assignat cap escena de men� principal!");
        }
    }
}
