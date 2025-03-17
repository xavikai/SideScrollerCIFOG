using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    [Header("Vida")]
    public float currentHealth;
    public float maxHealth = 100f;

    [Header("Estamina")]
    public float currentStamina;
    public float maxStamina = 100f;

    [Tooltip("Velocitat a la qual es consumeix l'estamina mentre esprintes")]
    public float staminaDrainRate = 10f;

    [Tooltip("Velocitat de regeneració de l'estamina quan no esprintes")]
    public float staminaRegenRate = 5f;

    [Header("Monedes")]
    public int currentCoins = 0;

    [Header("Configuració singleton")]
    [Tooltip("Activa per mantenir aquest objecte persistent entre escenes")]
    public bool isPersistent = true;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (isPersistent)
        {
            DontDestroyOnLoad(gameObject);
        }

        ResetPlayerState(); // Inicialitza els valors al començar
    }

    private void Update()
    {
        RegenerateStamina(Time.deltaTime);
    }

    // 🔵 Inicialitza o reseteja l'estat del jugador (quan es comença un nivell nou)
    public void ResetPlayerState()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentCoins = 0;
    }

    // 🔵 Assigna estat manualment (opcional)
    public void SetPlayerState(float health, float stamina, int coins)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        currentStamina = Mathf.Clamp(stamina, 0, maxStamina);
        currentCoins = coins;
    }

    // 🔵 Comprova si hi ha prou estamina per una acció i la consumeix si és possible
    public bool TryUseStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            return true;
        }

        return false;
    }

    // 🔵 Regenera estamina (es crida cada frame al Update)
    private void RegenerateStamina(float deltaTime)
    {
        currentStamina = Mathf.Min(currentStamina + staminaRegenRate * deltaTime, maxStamina);
    }

    // 🔵 Afegeix monedes
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        Debug.Log("Monedes actuals: " + currentCoins);
    }

    // 🔵 Rep dany i gestiona la mort
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Debug.Log("El jugador ha mort!");
            // Aquí pots notificar al GameManager o al PlayerUIController que estàs mort.
        }
    }
}
