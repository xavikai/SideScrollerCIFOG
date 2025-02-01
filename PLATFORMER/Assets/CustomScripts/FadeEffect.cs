using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    public Image fadeImage;          // Imatge de fons negre que cobreix la pantalla
    public bool fadeInStart = true;    // Si està activada, s'executa el fade in al carregar el nivell
    public float fadeInDuration = 1f;  // Durada del fade in

    public bool fadeOutEnd = true;     // Si està activada, s'executa el fade out al canviar de nivell
    public float fadeOutDuration = 1f; // Durada del fade out

    void Start()
    {
        // Si volem fade in, s'executa la corutina
        if (fadeInStart)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            // En cas contrari, la imatge és totalment transparent
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    // Transició de negre a transparent
    public IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color c = fadeImage.color;

        // Inicia amb la pantalla totalment opaca
        c.a = 1f;
        fadeImage.color = c;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = 1 - Mathf.Clamp01(elapsedTime / fadeInDuration);
            fadeImage.color = c;
            yield return null;
        }
        // Força la transparència completa al final
        c.a = 0f;
        fadeImage.color = c;
    }

    // Transició de transparent a negre
    public IEnumerator FadeOut()
    {
        // Si fadeOutEnd està desactivat, no executem cap transició
        if (!fadeOutEnd)
        {
            yield break;
        }

        float elapsedTime = 0f;
        Color c = fadeImage.color;

        // Inicia amb la pantalla transparent
        c.a = 0f;
        fadeImage.color = c;

        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / fadeOutDuration);
            fadeImage.color = c;
            yield return null;
        }
        // Força la pantalla opaca al final
        c.a = 1f;
        fadeImage.color = c;
    }
}
