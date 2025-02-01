using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Mètode per carregar l'escena del joc
    public void PlayGame()
    {
        SceneManager.LoadScene("SideScroller"); // Substitueix "NivellDelJoc" pel nom de la teva escena de joc
    }

    // Mètode per obrir el menú d'opcions
    public void OpenOptions()
    {
        // Aquí pots carregar una escena d'opcions o mostrar/amagar elements d'opcions
        Debug.Log("Opcions obertes");
    }

    // Mètode per sortir del joc
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Joc tancat");
    }
}