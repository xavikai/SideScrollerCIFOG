using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace StarterAssets
{
    public class PlayerUIController : MonoBehaviour
    {
        [Header("UI")]
        public Slider healthSlider;
        public Slider staminaSlider;
        public TMP_Text coinText;

        [Header("Vida i Estamina")]
        public float maxHealth = 100f;
        public float maxStamina = 100f;
        public float runStaminaDrainRate = 10f;
        public float staminaRegenRate = 5f;

#if UNITY_EDITOR
        [Header("Scenes (Editor)")]
        public SceneAsset gameOverSceneAsset;
        public SceneAsset youWinSceneAsset;
#endif
        [HideInInspector] public string gameOverSceneName;
        [HideInInspector] public string youWinSceneName;

        [Header("Condicions de Victòria")]
        public int requiredCoinsToWin = 10;
        public bool hasReachedWinZone = false;

        // Variables internes
        private float currentHealth;
        private float currentStamina;
        private int currentCoins;

        // Propietat pública per accedir al nombre de monedes
        public int CurrentCoins => currentCoins;

        public bool CanSprint => currentStamina > 0;

        void Start()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            currentCoins = 0;

            if (healthSlider != null)
            {
                healthSlider.minValue = 0;
                healthSlider.maxValue = maxHealth;
                healthSlider.value = currentHealth;
            }

            if (staminaSlider != null)
            {
                staminaSlider.minValue = 0;
                staminaSlider.maxValue = maxStamina;
                staminaSlider.value = currentStamina;
            }

            UpdateCoinUI();
        }

        void Update()
        {
            HandleStamina();
            CheckWinCondition();
        }

        private void HandleStamina()
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentStamina -= runStaminaDrainRate * Time.deltaTime;
                if (currentStamina < 0) currentStamina = 0;
            }
            else
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                if (currentStamina > maxStamina) currentStamina = maxStamina;
            }

            if (staminaSlider != null)
            {
                staminaSlider.value = currentStamina;
            }
        }

        public void UpdateHealth(float newHealth)
        {
            currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
            if (healthSlider != null) healthSlider.value = currentHealth;

            Debug.Log("Vida restant: " + currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;
            if (healthSlider != null) healthSlider.value = currentHealth;

            Debug.Log("Vida restant: " + currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void UpdateStamina(float newStamina)
        {
            currentStamina = Mathf.Clamp(newStamina, 0, maxStamina);
            if (staminaSlider != null) staminaSlider.value = currentStamina;
        }

        public void AddCoins(int amount)
        {
            currentCoins += amount;
            UpdateCoinUI();
            Debug.Log("Monedes actuals: " + currentCoins);
        }

        public void RemoveCoins(int amount)
        {
            currentCoins -= amount;
            if (currentCoins < 0) currentCoins = 0;
            UpdateCoinUI();
        }

        private void UpdateCoinUI()
        {
            if (coinText != null)
            {
                coinText.text = "Monedes: " + currentCoins.ToString();
            }
        }

        private void CheckWinCondition()
        {
            if (hasReachedWinZone && currentCoins >= requiredCoinsToWin)
            {
                Win();
            }
        }

        private void Die()
        {
            Debug.Log("El jugador ha mort. Carregant Game Over...");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (!string.IsNullOrEmpty(gameOverSceneName))
            {
                SceneManager.LoadScene(gameOverSceneName);
            }
            else
            {
                Debug.LogWarning("No s'ha assignat l'asset de Game Over.");
            }
        }

        public void Win() // Ara públic
        {
            Debug.Log("El jugador ha guanyat! Carregant escena de victòria...");
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            if (!string.IsNullOrEmpty(youWinSceneName))
            {
                SceneManager.LoadScene(youWinSceneName);
            }
            else
            {
                Debug.LogWarning("No s'ha assignat l'asset de You Win.");
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (gameOverSceneAsset != null)
            {
                gameOverSceneName = gameOverSceneAsset.name;
            }

            if (youWinSceneAsset != null)
            {
                youWinSceneName = youWinSceneAsset.name;
            }
        }
#endif
    }
}
