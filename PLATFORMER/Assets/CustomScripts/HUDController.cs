using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    public PlayerStateManager playerStateManager;

    public Slider healthSlider;
    public Slider staminaSlider;
    public TMP_Text coinText;

    private void Update()
    {
        if (playerStateManager == null) return;

        healthSlider.value = playerStateManager.currentHealth;
        staminaSlider.value = playerStateManager.currentStamina;
        coinText.text = $"Monedes: {playerStateManager.currentCoins}";
    }
}
