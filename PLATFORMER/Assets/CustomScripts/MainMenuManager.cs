using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // M�tode per carregar l'escena del joc
    public void PlayGame()
    {
        SceneManager.LoadScene("SideScroller"); // Substitueix "NivellDelJoc" pel nom de la teva escena de joc
    }

    // M�tode per obrir el men� d'opcions
    public void OpenOptions()
    {
        // Aqu� pots carregar una escena d'opcions o mostrar/amagar elements d'opcions
        Debug.Log("Opcions obertes");
    }

    // M�tode per sortir del joc
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Joc tancat");
    }
}