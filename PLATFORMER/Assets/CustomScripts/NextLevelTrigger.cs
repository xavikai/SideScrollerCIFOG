using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor; // Necessari per usar SceneAsset a l'Inspector
#endif

public class NextLevelTrigger : MonoBehaviour
{
    [Header("Següent escena")]
#if UNITY_EDITOR
    [Tooltip("Arrossega aquí la següent escena")]
    public SceneAsset nextLevelScene; // Només disponible a l'editor
#endif

    [HideInInspector]
    public string nextLevelSceneName;

    [Header("Requisits per passar de nivell")]
    public bool requiresCoins = false;   // El game designer decideix si són necessàries monedes
    public int requiredCoins = 10;       // Monedes mínimes per passar de nivell si s'activa requiresCoins

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (nextLevelScene != null)
        {
            nextLevelSceneName = nextLevelScene.name;
            EditorUtility.SetDirty(this);
        }
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log($"➡️ Jugador ha entrat al trigger del següent nivell: {nextLevelSceneName}");

        // Si es necessita un nombre de monedes determinat
        if (requiresCoins)
        {
            int playerCoins = PlayerStateManager.Instance.currentCoins;

            if (playerCoins < requiredCoins)
            {
                Debug.LogWarning($"🚫 No tens prou monedes per passar de nivell! Tens {playerCoins} / {requiredCoins}");
                return; // Bloqueja el canvi de nivell
            }

            Debug.Log($"✅ Tens prou monedes ➜ {playerCoins} / {requiredCoins}. Passant al següent nivell...");
        }

        if (!string.IsNullOrEmpty(nextLevelSceneName))
        {
            GameManager.Instance.StartNextLevel(nextLevelSceneName);
        }
        else
        {
            Debug.LogError("❌ No s'ha definit el nom de la següent escena!");
        }
    }
}
