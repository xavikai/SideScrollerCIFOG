using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Canvas del Menú de Pausa")]
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Joc reprès!");
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Joc pausat!");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Reiniciant el nivell...");
        GameManager.Instance.RestartLevel();
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Debug.Log("Tornant al menú principal...");
        GameManager.Instance.LoadScene("MainMenu");
    }
}
