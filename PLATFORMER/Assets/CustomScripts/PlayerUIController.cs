using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerUIController : MonoBehaviour
{
    [Header("HUD Elements")]
    public Slider healthSlider;
    public Slider staminaSlider;
    public TMP_Text coinText;

    private PlayerStateManager playerState;

    private void Start()
    {
        StartCoroutine(WaitForPlayerStateManager());
    }

    private IEnumerator WaitForPlayerStateManager()
    {
        while (PlayerStateManager.Instance == null)
        {
            yield return null;
        }

        playerState = PlayerStateManager.Instance;
        SetupUI();
    }

    private void SetupUI()
    {
        if (healthSlider != null)
            healthSlider.maxValue = playerState.maxHealth;

        if (staminaSlider != null)
            staminaSlider.maxValue = playerState.maxStamina;

        UpdateUI();
    }

    private void Update()
    {
        if (playerState == null) return;

        UpdateUI();
    }

    private void UpdateUI()
    {
        healthSlider.value = playerState.currentHealth;
        staminaSlider.value = playerState.currentStamina;
        coinText.text = "Monedes: " + playerState.currentCoins;
    }
}
