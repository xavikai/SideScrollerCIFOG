using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public float currentHealth;
    public float currentStamina;
    public int currentCoins;

    public float maxStamina = 100f;
    public float staminaDrainRate = 10f;

    public bool CanSprint()
    {
        return currentStamina > 0;
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
    }
}
