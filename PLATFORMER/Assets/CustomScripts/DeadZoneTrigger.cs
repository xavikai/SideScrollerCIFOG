using UnityEngine;

public class DeadZoneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("☠️ El jugador ha caigut a la DeadZone!");

            // Reproduir so de caiguda via AudioManager
            AudioManager.Instance?.PlaySound(AudioManager.Instance.deadZoneFallSound, transform.position);

            if (PlayerStateManager.Instance != null)
            {
                PlayerStateManager.Instance.TakeDamage(PlayerStateManager.Instance.currentHealth);
            }
            else
            {
                Debug.LogWarning("❗ PlayerStateManager no trobat!");
            }
        }
    }
}
