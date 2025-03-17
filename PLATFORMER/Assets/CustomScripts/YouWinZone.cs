using UnityEngine;

namespace StarterAssets
{
    public class YouWinZone : MonoBehaviour
    {
        [Tooltip("Assigna el PlayerUIController manualment des de l'Inspector")]
        public PlayerUIController playerUIController;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log("Jugador ha entrat a la zona de YouWin!");

                if (playerUIController != null)
                {
                    playerUIController.hasReachedWinZone = true;

                    if (playerUIController.CurrentCoins >= playerUIController.requiredCoinsToWin)
                    {
                        playerUIController.Win();
                    }
                    else
                    {
                        Debug.Log("Encara falten monedes! Monedes actuals: " + playerUIController.CurrentCoins);
                    }
                }
                else
                {
                    Debug.LogWarning("PlayerUIController no assignat!");
                }
            }
        }
    }
}
