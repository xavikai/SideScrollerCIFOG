using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseMenuController : MonoBehaviour
{
    // Panell del menú de pausa. Assigna'l des de l'Inspector.
    public GameObject pauseMenuUI;

    // Referència al CinemachineBrain per gestionar el moviment de la càmera.
    public CinemachineBrain cinemachineBrain;

    // Camp per assignar la SceneAsset de l'escena del menú principal (només visible a l'Editor).
#if UNITY_EDITOR
    public SceneAsset mainMenuSceneAsset;
#endif

    // Aquest camp guardarà el nom de l'escena del menú principal.
    [HideInInspector]
    public string mainMenuSceneName;

    private bool isPaused = false;

    void Update()
    {
        // Detecta la tecla Escape per alternar la pausa.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    // Reanuda el joc.
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Tornar a activar el CinemachineBrain per reprendre el moviment de la càmera.
        if (cinemachineBrain != null)
            cinemachineBrain.enabled = true;

        // Gestionar el cursor: amagar-lo i bloquejar-lo (pot variar segons les necessitats del joc).
        Cursor.lockState = CursorLockMode.Locked; // o utilitza CursorLockMode.Confined segons el disseny.
        Cursor.visible = false;
    }

    // Pausa el joc.
    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Desactivar el CinemachineBrain per congelar el moviment de la càmera.
        if (cinemachineBrain != null)
            cinemachineBrain.enabled = false;

        // Gestionar el cursor: fer-lo visible i desblocar-lo per interactuar amb el menú.
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Carrega l'escena del menú principal.
    public void LoadMainMenu()
    {
        // Restableix el temps abans de canviar d'escena.
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Si s'ha assignat la SceneAsset del menú principal, actualitza el nom de l'escena.
        if (mainMenuSceneAsset != null)
            mainMenuSceneName = mainMenuSceneAsset.name;
    }
#endif
}
