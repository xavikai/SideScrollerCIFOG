using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [Header("Configuració del Dany")]
    public float damageAmount = 10f;
    public bool damageOverTime = false;
    public float damageRate = 1f; // cada quant temps fa dany si es damageOverTime
    private float nextDamageTime = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aplicar el dany immediatament al entrar
            DealDamage();

            if (damageOverTime)
            {
                nextDamageTime = Time.time + damageRate;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && damageOverTime)
        {
            if (Time.time >= nextDamageTime)
            {
                DealDamage();
                nextDamageTime = Time.time + damageRate;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset del temporitzador o altres accions si cal
        }
    }

    private void DealDamage()
    {
        if (PlayerStateManager.Instance != null)
        {
            PlayerStateManager.Instance.TakeDamage(damageAmount);

            Debug.Log($"👊 Dany aplicat al jugador: {damageAmount}. Vida actual: {PlayerStateManager.Instance.currentHealth}");

            if (PlayerStateManager.Instance.currentHealth <= 0f)
            {
                Debug.Log("💀 El jugador ha mort.");
                // Aquí pots cridar GameManager.Instance.LoadScene("GameOverScene");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ No s'ha trobat el PlayerStateManager!");
        }
    }
}
