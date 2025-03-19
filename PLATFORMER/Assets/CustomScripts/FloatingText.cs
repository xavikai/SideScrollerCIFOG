using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    [Header("Moviment i Desaparició")]
    public float floatSpeed = 1f;
    public float fadeDuration = 1f;
    public Vector3 floatDirection = Vector3.up;

    [Header("Text i Color")]
    public string displayText = "+50"; // Text predeterminat
    public Color textColor = Color.white; // Color predeterminat del text

    private TMP_Text textMesh;
    private Color originalColor;
    private float elapsedTime = 0f;

    private void Awake()
    {
        textMesh = GetComponentInChildren<TMP_Text>();

        if (textMesh != null)
        {
            textMesh.text = displayText;
            textMesh.color = textColor;
            originalColor = textColor;
        }
        else
        {
            Debug.LogError("❗ No s'ha trobat el TMP_Text al FloatingText!");
        }
    }

    private void Update()
    {
        // Mou el text cap amunt
        transform.position += floatDirection * floatSpeed * Time.deltaTime;

        // Incrementa el temps i calcula l'opacitat
        elapsedTime += Time.deltaTime;
        float fadeAmount = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));

        if (textMesh != null)
        {
            Color newColor = originalColor;
            newColor.a = fadeAmount;
            textMesh.color = newColor;
        }

        // Destrueix l'objecte quan s'ha esvaït del tot
        if (elapsedTime >= fadeDuration)
        {
            Destroy(gameObject);
        }
    }

    // 🔥 Mètode per configurar el text i el color des del RegeneratorItem
    public void SetupText(string newText, Color newColor)
    {
        if (textMesh != null)
        {
            textMesh.text = newText;
            textMesh.color = newColor;
            originalColor = newColor; // Per al fade
        }
    }
}
