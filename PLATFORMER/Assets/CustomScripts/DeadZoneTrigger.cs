using UnityEngine;

public class DeadZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("☠️ El jugador ha caigut a la DeadZone!");

            // Comprova si hi ha un PlayerStateManager actiu
            if (PlayerStateManager.Instance != null)
            {
                // Redueix la vida a zero ➜ Morirà automàticament si tens aquest control
                PlayerStateManager.Instance.TakeDamage(PlayerStateManager.Instance.currentHealth);
            }
            else
            {
                Debug.LogWarning("❗ PlayerStateManager no trobat!");
            }
        }
    }
}
