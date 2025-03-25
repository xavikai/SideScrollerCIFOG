using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [Header("Moviment i Desaparició")]
    public float floatSpeed = 1f;
    public float fadeDuration = 1f;
    public Vector3 floatDirection = Vector3.up;

    [Header("Referència explícita al TextFeedback")]
    public TMP_Text textFeedback;  // Referència directa al TextMeshPro que vols controlar.

    private Color originalColor;
    private float elapsedTime = 0f;

    private void Awake()
    {
        // Assegura't que tenim referència
        if (textFeedback == null)
        {
            Debug.LogError("❗ Assegura't d'assignar el TMP_Text 'TextFeedback' al FloatingText!");
        }
    }

    private void Update()
    {
        // Mou el text cap amunt
        transform.position += floatDirection * floatSpeed * Time.deltaTime;

        elapsedTime += Time.deltaTime;
        float fadeAmount = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));

        if (textFeedback != null)
        {
            Color fadedColor = originalColor;
            fadedColor.a = fadeAmount;

            // ✅ Assignació directa al vertex color
            textFeedback.color = fadedColor;
        }

        if (elapsedTime >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }

    public void SetupText(string newText, Color newVertexColor)
    {
        if (textFeedback != null)
        {
            textFeedback.text = newText;

            // Forcem el Face Color del Material a blanc (només per seguretat)
            textFeedback.fontMaterial.SetColor(ShaderUtilities.ID_FaceColor, Color.white);

            // Assignem el vertex color
            originalColor = newVertexColor;
            textFeedback.color = originalColor;

            Debug.Log($"✅ SetupText ➜ Text: {newText}, Color: {originalColor}");
        }
    }
}
