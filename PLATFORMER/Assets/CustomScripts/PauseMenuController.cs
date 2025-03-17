using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenuUI.activeSelf)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        GameManager.Instance.RestartLevel();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        GameManager.Instance.GoToMainMenu();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
}
