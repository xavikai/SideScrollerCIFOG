using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    public Image fadeImage;          // Imatge de fons negre que cobreix la pantalla
    public bool fadeInStart = true;    // Si est� activada, s'executa el fade in al carregar el nivell
    public float fadeInDuration = 1f;  // Durada del fade in

    public bool fadeOutEnd = true;     // Si est� activada, s'executa el fade out al canviar de nivell
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
            // En cas contrari, la imatge �s totalment transparent
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }
    }

    // Transici� de negre a transparent
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
        // For�a la transpar�ncia completa al final
        c.a = 0f;
        fadeImage.color = c;
    }

    // Transici� de transparent a negre
    public IEnumerator FadeOut()
    {
        // Si fadeOutEnd est� desactivat, no executem cap transici�
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
        // For�a la pantalla opaca al final
        c.a = 1f;
        fadeImage.color = c;
    }
}
