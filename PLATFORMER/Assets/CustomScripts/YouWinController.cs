using UnityEngine;
using UnityEngine.UI;

public class YouWinController : MonoBehaviour
{
    public Button mainMenuButton;
    public Button exitGameButton;

    private void Start()
    {
        // Mostrem el cursor per interactuar amb la UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
        }
        else
        {
            Debug.LogWarning("❗ mainMenuButton no assignat en YouWinController!");
        }

        if (exitGameButton != null)
        {
            exitGameButton.onClick.AddListener(ExitGame);
        }
        else
        {
            Debug.LogWarning("❗ exitGameButton no assignat en YouWinController!");
        }
    }

    private void GoToMainMenu()
    {
        Debug.Log("🏠 Tornant al Main Menu des de YouWin");
        GameManager.Instance.GoToMainMenu();
    }

    private void ExitGame()
    {
        Debug.Log("🚪 Sortint del joc des de YouWin");

#if UNITY_EDITOR
        // Això funciona dins l'Editor per simular la sortida
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Això funciona en una build real
        Application.Quit();
#endif
    }
}
