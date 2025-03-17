using UnityEngine;
using UnityEngine.UI;

public class PlayerUIController : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;
    public Text coinText;

    private void Start()
    {
        healthSlider.maxValue = PlayerStateManager.Instance.maxHealth;
        staminaSlider.maxValue = PlayerStateManager.Instance.maxStamina;
    }

    private void Update()
    {
        UpdateHealthUI();
        UpdateStaminaUI();
        UpdateCoinsUI();
    }

    private void UpdateHealthUI()
    {
        if (healthSlider != null)
        {
            healthSlider.value = PlayerStateManager.Instance.currentHealth;
        }
    }

    private void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = PlayerStateManager.Instance.currentStamina;
        }
    }

    private void UpdateCoinsUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + PlayerStateManager.Instance.currentCoins.ToString();
        }
    }
}
