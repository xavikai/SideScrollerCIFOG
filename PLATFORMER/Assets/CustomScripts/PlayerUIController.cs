using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUIController : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider healthSlider;
    public Slider staminaSlider;
    public TMP_Text coinText;

    [Header("Player Stats")]
    public float maxHealth = 100f;
    public float maxStamina = 100f;

    private float currentHealth;
    private float currentStamina;
    private int currentCoins;

    // Propietat pública per accedir a les monedes
    public int CurrentCoins => currentCoins;

    private void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentCoins = 0;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }

        UpdateCoinUI();
    }

    private void Update()
    {
        // Aquí podries posar control de regeneració de stamina si ho necessites
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("El jugador ha mort!");
            // Crida al GameManager si vols carregar GameOver
            GameManager.Instance.LoadScene("GameOver");
        }
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        if (staminaSlider != null)
            staminaSlider.value = currentStamina;
    }

    public void RegenerateStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        if (staminaSlider != null)
            staminaSlider.value = currentStamina;
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "Monedes: " + currentCoins;
        }
    }

    // Per carregar dades guardades (opcional)
    public void SetValues(float health, float stamina, int coins)
    {
        currentHealth = health;
        currentStamina = stamina;
        currentCoins = coins;

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (staminaSlider != null)
            staminaSlider.value = currentStamina;

        UpdateCoinUI();
    }
}
