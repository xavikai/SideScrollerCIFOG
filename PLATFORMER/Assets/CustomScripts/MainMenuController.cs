using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("Botons del Main Menu")]
    public Button playButton;
    public Button quitButton;

    private void Start()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(() =>
            {
                Debug.Log("🎮 Play Button Premut");
                GameManager.Instance.StartNewGame();
            });
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(() =>
            {
                Debug.Log("🚪 Quit Button Premut");
                GameManager.Instance.QuitGame();
            });
        }
    }
}
