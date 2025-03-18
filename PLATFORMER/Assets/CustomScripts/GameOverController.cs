using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [Header("Botons GameOver")]
    public Button restartButton;
    public Button mainMenuButton;

    private void Start()
    {
        // Assegura que els botons estan assignats
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartLevel);

        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    public void RestartLevel()
    {
        Debug.Log("🔄 Restart Level des de GameOver");
        GameManager.Instance.RestartLastLevel();
    }


    public void GoToMainMenu()
    {
        Debug.Log("🏠 Tornant al Main Menu des de GameOver");
        GameManager.Instance.GoToMainMenu();
    }
}
