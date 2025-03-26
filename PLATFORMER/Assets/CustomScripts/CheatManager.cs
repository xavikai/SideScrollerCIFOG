using UnityEngine;

public class CheatManager : MonoBehaviour
{
    public static CheatManager Instance;

    [Header("Cheat Settings")]
    public bool cheatsEnabled = true;
    public bool debugModeOnly = true; // Només en mode editor o debug

    [Header("FloatingText Settings")]
    public GameObject floatingTextPrefab;
    public Transform floatingTextSpawnPoint; // Opcional, sinó apareixerà davant la càmera
    public Color cheatTextColor = Color.cyan;
    public float floatingTextFontSize = 2f; // Mida del text

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
        DetectFunctionKeyCheats();
    }

    private void DetectInput()
    {
        foreach (char c in Input.inputString)
        {
            inputBuffer += c;
            bufferTimer = bufferClearTime;

            if (inputBuffer.StartsWith("l") && inputBuffer.Length > 1)
            {
                string levelNumber = inputBuffer.Substring(1);

                if (int.TryParse(levelNumber, out int levelIndex))
                {
                    string levelName = $"Level{levelIndex}";
                    Debug.Log($"🚀 Cheat activat: saltant a {levelName}");
                    ShowCheatStaticText($"LEVEL {levelIndex}");

                    StartCoroutine(DelayedLevelLoad(levelName));

                    inputBuffer = "";
                    return;
                }
            }
        }

        if (bufferTimer > 0)
        {
            bufferTimer -= Time.deltaTime;
        }
        else
        {
            inputBuffer = "";
        }
    }

    private void DetectFunctionKeyCheats()
    {
        var player = PlayerStateManager.Instance;
        if (player == null) return;

        if (Input.GetKeyDown(KeyCode.F6))
        {
            player.currentHealth = player.maxHealth;
            ShowCheatStaticText($"HEALTH MAX!");
            Debug.Log("❤️ Vida restaurada al màxim.");
        }

        if (Input.GetKeyDown(KeyCode.F7))
        {
            player.currentStamina = player.maxStamina;
            ShowCheatStaticText($"STAMINA MAX!");
            Debug.Log("⚡ Estamina restaurada al màxim.");
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            player.AddCoins(1);
            ShowCheatStaticText($"+1 COIN");
            Debug.Log($"🪙 Moneda afegida. Total: {player.currentCoins}");
        }
    }

    private System.Collections.IEnumerator DelayedLevelLoad(string levelName)
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.StartNextLevel(levelName);
    }

    private void ShowCheatStaticText(string message)
    {
        if (floatingTextPrefab == null)
        {
            Debug.LogWarning("❗ Prefab de FloatingText no assignat al CheatManager!");
            return;
        }

        Vector3 spawnPosition = floatingTextSpawnPoint != null
            ? floatingTextSpawnPoint.position
            : Camera.main.transform.position + Camera.main.transform.forward * 2f;

        GameObject textObj = Instantiate(floatingTextPrefab, spawnPosition, Quaternion.identity);

        FloatingText floatingTextScript = textObj.GetComponent<FloatingText>();

        if (floatingTextScript != null)
        {
            floatingTextScript.SetupText(message, cheatTextColor);
            floatingTextScript.textFeedback.fontSize = floatingTextFontSize;
            floatingTextScript.floatSpeed = 0f;
            floatingTextScript.fadeDuration = 1.4f;

            Debug.Log($"✅ Cheat Static Text: {message}");
        }
        else
        {
            Debug.LogError("❌ El prefab de FloatingText no té el component FloatingText!");
        }
    }
}
