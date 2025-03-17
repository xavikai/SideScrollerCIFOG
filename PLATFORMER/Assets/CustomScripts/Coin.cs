using UnityEngine;

namespace StarterAssets
{
    public class Coin : MonoBehaviour
    {
        [Tooltip("Quantes monedes suma aquesta moneda?")]
        public int coinValue = 1;

        [Tooltip("Opcional: efecte de so quan es recull")]
        public AudioClip pickupSound;

        [Tooltip("Arrossega aqu� el PlayerUIController (des de l'Inspector)")]
        public PlayerUIController playerUIController;

        private void OnTriggerEnter(Collider other)
        {
            // Comprovem si el collider que entra t� el tag "Player"
            if (other.CompareTag("Player"))
            {
                Debug.Log("Jugador ha entrat en el trigger de la moneda!");

                // Si hem assignat la refer�ncia del PlayerUIController
                if (playerUIController != null)
                {
                    // Afegim les monedes al controlador
                    playerUIController.AddCoins(coinValue);
                    Debug.Log($"Afegint {coinValue} moneda/es. Total: {playerUIController.CurrentCoins}");

                    // Reprodueix un so si hi ha �udio assignat
                    if (pickupSound != null)
                    {
                        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                    }

                    // Destrueix l'objecte moneda despr�s de recollir-la
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
