using UnityEngine;

namespace StarterAssets
{
    public class YouWinZone : MonoBehaviour
    {
        [Tooltip("Referència al PlayerUIController (arrossega-ho des de l'Inspector)")]
        public PlayerUIController playerUIController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Jugador ha entrat a la zona de YouWin!");

                if (playerUIController != null)
                {
                    if (playerUIController.CurrentCoins >= playerUIController.requiredCoinsToWin)
                    {
                        Debug.Log("Jugador ha complert totes les condicions! YOU WIN!");
                        playerUIController.Win();
                    }
                    else
                    {
                        int coinsLeft = playerUIController.requiredCoinsToWin - playerUIController.CurrentCoins;
                        Debug.Log($"Et falten {coinsLeft} monedes per guanyar. Torna quan les tinguis!");
                        // Aquí pots afegir codi per mostrar un missatge en pantalla si vols
                    }
                }
                else
                {
                    Debug.LogWarning("No s'ha assignat el PlayerUIController al YouWinZone!");
                }
            }
        }
    }
}
