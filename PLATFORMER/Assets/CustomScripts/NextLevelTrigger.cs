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

    private void OnValidate()
    {
#if UNITY_EDITOR
        // Aquesta funció s'executa quan canvies un valor a l'Inspector
        if (nextLevelScene != null)
        {
            nextLevelSceneName = nextLevelScene.name;
            EditorUtility.SetDirty(this); // Marca el component com a modificat per guardar el valor
        }
#endif
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"➡️ Jugador ha entrat al trigger del següent nivell: {nextLevelSceneName}");

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
}
