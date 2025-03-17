using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button restartLevelButton;
    public Button mainMenuButton;

    void Start()
    {
        if (restartLevelButton != null)
        {
            restartLevelButton.onClick.AddListener(RestartLevel);
        }
        else
        {
            Debug.LogWarning("Falta assignar el botó Restart Level al GameOverController!");
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(BackToMainMenu);
        }
        else
        {
            Debug.LogWarning("Falta assignar el botó Main Menu al GameOverController!");
        }
    }

    private void RestartLevel()
    {
        GameManager.Instance.RestartLevel();
    }

    private void BackToMainMenu()
    {
        GameManager.Instance.ResetPlayerStats();
        GameManager.Instance.LoadScene("MainMenu");
    }
}
