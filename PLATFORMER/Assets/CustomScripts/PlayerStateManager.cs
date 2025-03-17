using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    [Header("Vida")]
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Estamina")]
    public float maxStamina = 100f;
    public float currentStamina;
    public float staminaDrainRate = 10f;
    public float staminaRegenRate = 5f;

    [Header("Monedes")]
    public int currentCoins = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        ResetStats();
    }

    private void Update()
    {
        RegenerateStamina();
    }

    public void ResetStats()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentCoins = 0;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0f)
            currentHealth = 0f;

        Debug.Log($"Player damaged! Vida actual: {currentHealth}");

        if (currentHealth <= 0f)
        {
            Debug.Log("Jugador mort!");
            // Aquí pots cridar al GameManager per mostrar el Game Over
        }
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        Debug.Log($"Has recollit {amount} monedes. Total: {currentCoins}");
    }

    public bool TryUseStamina(float amount)
    {
        if (currentStamina > 0f)
        {
            currentStamina -= amount * Time.deltaTime;
            if (currentStamina < 0f) currentStamina = 0f;
            return true;
        }
        return false;
    }

    private void RegenerateStamina()
    {
        if (!StarterAssets.ThirdPersonController.IsSprinting)
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            if (currentStamina > maxStamina)
                currentStamina = maxStamina;
        }
    }
}
