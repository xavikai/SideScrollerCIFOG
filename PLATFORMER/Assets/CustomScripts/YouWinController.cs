using StarterAssets;
using UnityEngine;

public class YouWinZone : MonoBehaviour
{
    [Header("Requisits de victòria")]
    public int requiredCoins = 4;  // Nombre mínim de monedes necessàries per guanyar

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
                Debug.LogWarning("El Player no té el PlayerUIController assignat!");
            }
        }
    }
}
