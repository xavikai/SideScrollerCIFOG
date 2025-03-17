using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public float damageAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateManager.Instance.TakeDamage(damageAmount);
        }
    }
}
