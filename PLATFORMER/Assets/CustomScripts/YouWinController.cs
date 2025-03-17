using UnityEngine;
using UnityEngine.UI;

public class YouWinController : MonoBehaviour
{
    [Header("Botons")]
    public Button restartLevelButton;
    public Button mainMenuButton;

    private void Start()
    {
        // Mostrar el cursor per assegurar que es pot interactuar
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Afegir listeners als botons
        restartLevelButton.onClick.AddListener(RestartLevel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void RestartLevel()
    {
        Debug.Log("🔄 Reiniciant el nivell!");
        GameManager.Instance.RestartLevel();
    }

    public void GoToMainMenu()
    {
        Debug.Log("🏠 Tornant al menú principal!");
        GameManager.Instance.LoadScene("MainMenu");

        // Reiniciem stats si tornem al menú principal
        GameManager.Instance.ResetPlayerStats();
    }
}
