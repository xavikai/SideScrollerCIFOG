using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameOverController : MonoBehaviour
{
    [Header("Botons")]
    [Tooltip("Botó per reiniciar el joc")]
    public Button restartButton;

    [Tooltip("Botó per tornar al menú principal")]
    public Button mainMenuButton;

    [Header("Assigna les escenes arrossegant-les aquí (Editor només)")]
#if UNITY_EDITOR
    public SceneAsset gameScene;         // Escena del joc
    public SceneAsset mainMenuScene;     // Escena del menú principal
#endif

    [Header("Noms de les escenes (es posen sols, no toquis!)")]
    public string gameSceneName;         // Es fa servir en runtime
    public string mainMenuSceneName;

#if UNITY_EDITOR
    // Aquesta funció es crida automàticament quan canvies alguna cosa a l'inspector
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

        // Assigna els listeners als botons (millor aquí que des de l'inspector)
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
        else
            Debug.LogWarning("No s'ha assignat el botó de restart.");

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        else
            Debug.LogWarning("No s'ha assignat el botó de menú principal.");
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
            // Mostrem el cursor al menú per si de cas
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
