using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [Header("Fade Settings")]
    public Image fadeImage;

    [Header("Durations (seconds)")]
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;

    [Header("Options")]
    public bool fadeEnabled = true;    // Permet activar/desactivar fade fàcilment (per debug o optimització)

    private Coroutine currentFadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;
        }

        if (fadeImage == null)
        {
            Debug.LogError("⚠️ FadeImage no assignat en el FadeManager!");
        }
    }

    private void Start()
    {
        // Només fem el fade in si està activat
        if (fadeEnabled)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        if (!fadeEnabled || fadeImage == null)
        {
            Debug.Log("⚡ Fade In desactivat o sense FadeImage.");
            return;
        }

        StartFade(1f, 0f, fadeInDuration);
    }

    public void FadeOut()
    {
        if (!fadeEnabled || fadeImage == null)
        {
            Debug.Log("⚡ Fade Out desactivat o sense FadeImage.");
            return;
        }

        StartFade(0f, 1f, fadeOutDuration);
    }

    // 👉 Sobrecàrrega per fer servir una duració custom temporal
    public void FadeIn(float customDuration)
    {
        if (!fadeEnabled || fadeImage == null)
        {
            Debug.Log("⚡ Fade In desactivat o sense FadeImage.");
            return;
        }

        StartFade(1f, 0f, customDuration);
    }

    public void FadeOut(float customDuration)
    {
        if (!fadeEnabled || fadeImage == null)
        {
            Debug.Log("⚡ Fade Out desactivat o sense FadeImage.");
            return;
        }

        StartFade(0f, 1f, customDuration);
    }

    private void StartFade(float startAlpha, float endAlpha, float duration)
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        currentFadeCoroutine = StartCoroutine(FadeRoutine(startAlpha, endAlpha, duration));
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color c = fadeImage.color;
        c.a = startAlpha;
        fadeImage.color = c;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);

            c.a = alpha;
            fadeImage.color = c;

            yield return null;
        }

        c.a = endAlpha;
        fadeImage.color = c;

        currentFadeCoroutine = null;
    }

    public bool IsFading()
    {
        return currentFadeCoroutine != null;
    }
}
