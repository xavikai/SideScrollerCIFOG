using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    public float maxHealth = 100f;
    public float currentHealth;

    public float maxStamina = 100f;
    public float currentStamina;

    public int currentCoins;

    public float staminaDrainRate = 10f; // Per l'sprint
    public float staminaRegenRate = 5f;

    public bool isFrozen = false;

    public bool CanMove => currentHealth > 0 && !isFrozen;

    // 🔸 Estats de partida al començament del nivell
    private float levelStartHealth;
    private float levelStartStamina;
    private int levelStartCoins;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentCoins = 0;

        // Guardem els valors inicials
        SaveLevelStartState();
    }

    private void Update()
    {
        RegenerateStamina();
    }

    private void RegenerateStamina()
    {
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("💀 Jugador ha mort!");
            GameManager.Instance.GoToGameOver();
        }
    }

    public bool TryUseStamina(float amount)
    {
        if (currentStamina >= amount)
        {
            currentStamina -= amount;
            return true;
        }
        return false;
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
    }

    public void SetPlayerState(float health, float stamina, int coins)
    {
        currentHealth = health;
        currentStamina = stamina;
        currentCoins = coins;
    }

    // 🔸 Guardar estat inicial del nivell
    public void SaveLevelStartState()
    {
        levelStartHealth = currentHealth;
        levelStartStamina = currentStamina;
        levelStartCoins = currentCoins;

        Debug.Log($"📝 Estat inicial guardat ➜ Vida: {levelStartHealth}, Estamina: {levelStartStamina}, Monedes: {levelStartCoins}");
    }

    // 🔸 Restaurar estat inicial del nivell
    public void RestoreLevelStartState()
    {
        SetPlayerState(levelStartHealth, levelStartStamina, levelStartCoins);
        Debug.Log($"🔄 Estat inicial restaurat ➜ Vida: {currentHealth}, Estamina: {currentStamina}, Monedes: {currentCoins}");
    }
}
