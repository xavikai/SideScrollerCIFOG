using StarterAssets;
using UnityEngine;

public class YouWinZone : MonoBehaviour
{
    [Header("Requisits de vict�ria")]
    public int requiredCoins = 4;  // Nombre m�nim de monedes necess�ries per guanyar

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerUIController playerUI = other.GetComponent<PlayerUIController>();

            if (playerUI != null)
            {
                if (playerUI.CurrentCoins >= requiredCoins)
                {
                    playerUI.hasReachedWinZone = true;
                    playerUI.Win();
                }
                else
                {
                    Debug.Log($"Encara et falten monedes! {playerUI.CurrentCoins}/{requiredCoins}");
                }
            }
            else
            {
                Debug.LogWarning("El Player no t� el PlayerUIController assignat!");
            }
        }
    }
}
