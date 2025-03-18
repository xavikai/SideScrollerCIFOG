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

    public bool isFrozen = false;  // Exemple si vols afegir condicions externes

    public bool CanMove => currentHealth > 0 && !isFrozen;

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
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("💀 Jugador ha mort!");
            GameManager.Instance.GoToGameOver();  // Exemple, canvia per GameOver si vols
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

    private void Update()
    {
        RegenerateStamina();
    }

    private void RegenerateStamina()
    {
        if (!Input.GetKey(KeyCode.LeftShift))  // Si no s'està esprintant
        {
            currentStamina += staminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }
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
}
