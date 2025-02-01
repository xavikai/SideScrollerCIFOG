using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor; // Necessari per utilitzar SceneAsset
#endif

public class MainMenuManager : MonoBehaviour
{
    public FadeEffect fadeEffect; // Assigna l'objecte FadeManager des de l'Inspector

#if UNITY_EDITOR
    public SceneAsset sceneToLoadAsset; // Permet arrossegar l'escena des del Project al Inspector
#endif

    private string sceneToLoad; // Nom de l'escena que es carregarà

    private void Awake()
    {
#if UNITY_EDITOR
        if (sceneToLoadAsset != null)
        {
            sceneToLoad = sceneToLoadAsset.name; // Obté el nom de l'escena seleccionada
        }
#endif
    }

    // Mètode per carregar l'escena del joc amb fade out
    public void PlayGame()
    {
        StartCoroutine(PlayGameWithFade());
    }

    private IEnumerator PlayGameWithFade()
    {
        // Fade out a negre
        yield return fadeEffect.FadeOut();

        // Carrega l'escena del joc
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneManager.LoadScene(sceneToLoad); // Utilitza la variable sceneToLoad
        }
        else
        {
            Debug.LogError("No s'ha especificat cap escena per carregar.");
        }
    }

    // Mètode per sortir del joc
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Joc tancat");
    }
}
