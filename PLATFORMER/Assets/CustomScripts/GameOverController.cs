using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [Tooltip("Botó per reiniciar el joc")]
    public Button restartButton;

    [Tooltip("Botó per tornar al menú principal")]
    public Button mainMenuButton;

    void Start()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void RestartGame()
    {
        // Carrega l'escena del joc (assegura't que el nom coincideix amb la teva escena de joc)
        SceneManager.LoadScene("NomDeLaTuaEscenaDeJoc");
    }

    public void GoToMainMenu()
    {
        // Carrega l'escena del menú principal
        SceneManager.LoadScene("NomDelMenuPrincipal");
    }
}
