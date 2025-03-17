using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (PlayerStateManager.Instance != null)
            {
                PlayerStateManager.Instance.AddCoins(coinValue);

                Debug.Log("Monedes totals: " + PlayerStateManager.Instance.currentCoins);

                // Destrueix la moneda després de recollir-la
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("⚠️ PlayerStateManager.Instance és NULL! Assegura't que existeix a l'escena.");
            }
        }
    }
}
