using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance;

    [Header("Cheat Settings")]
    public bool cheatsEnabled = true;
    public bool debugModeOnly = true; // Només en mode editor o debug

    private string inputBuffer = "";
    private float bufferClearTime = 2f; // temps per buidar el buffer
    private float bufferTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!cheatsEnabled) return;
        if (debugModeOnly && !Application.isEditor) return;

        DetectInput();
    }

    private void DetectInput()
    {
        foreach (char c in Input.inputString)
        {
            inputBuffer += c;
            bufferTimer = bufferClearTime;

            // Comprova si el buffer té un codi vàlid (ex: l4, l10, etc.)
            if (inputBuffer.StartsWith("l") && inputBuffer.Length > 1)
            {
                string levelNumber = inputBuffer.Substring(1);

                if (int.TryParse(levelNumber, out int levelIndex))
                {
                    string levelName = $"Level{levelIndex}";
                    Debug.Log($"🚀 Cheat activat: saltant a {levelName}");
                    GameManager.Instance.StartNextLevel(levelName);

                    inputBuffer = ""; // buida el buffer
                    return;
                }
            }
        }

        // Gestiona el temporitzador per buidar buffer
        if (bufferTimer > 0)
        {
            bufferTimer -= Time.deltaTime;
        }
        else
        {
            inputBuffer = "";
        }
    }
}
