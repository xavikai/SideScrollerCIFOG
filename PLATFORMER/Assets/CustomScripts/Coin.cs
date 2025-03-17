using UnityEngine;

namespace StarterAssets
{
    public class Coin : MonoBehaviour
    {
        [Tooltip("Quantes monedes suma aquesta moneda?")]
        public int coinValue = 1;

        [Tooltip("Opcional: efecte de so quan es recull")]
        public AudioClip pickupSound;

        [Tooltip("Arrossega aquí el PlayerUIController (des de l'Inspector)")]
        public PlayerUIController playerUIController;

        private void OnTriggerEnter(Collider other)
        {
            // Comprovem si el collider que entra té el tag "Player"
            if (other.CompareTag("Player"))
            {
                Debug.Log("Jugador ha entrat en el trigger de la moneda!");

                // Si hem assignat la referència del PlayerUIController
                if (playerUIController != null)
                {
                    // Afegim les monedes al controlador
                    playerUIController.AddCoins(coinValue);
                    Debug.Log($"Afegint {coinValue} moneda/es. Total: {playerUIController.CurrentCoins}");

                    // Reprodueix un so si hi ha àudio assignat
                    if (pickupSound != null)
                    {
                        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                    }

                    // Destrueix l'objecte moneda després de recollir-la
                    Destroy(gameObject);
                }
                else
                {
                    Debug.LogWarning("No s'ha assignat cap PlayerUIController a la moneda!");
                }
            }
        }
    }
}
