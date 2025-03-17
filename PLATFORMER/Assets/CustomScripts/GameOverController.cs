using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    public Button restartLevelButton;
    public Button mainMenuButton;

    private void Start()
    {
        restartLevelButton.onClick.AddListener(RestartLevel);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void RestartLevel()
    {
        GameManager.Instance.RestartLevel();
    }

    private void GoToMainMenu()
    {
        GameManager.Instance.GoToMainMenu();
    }
}
