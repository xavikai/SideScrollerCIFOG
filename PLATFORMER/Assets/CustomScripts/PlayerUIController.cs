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

        private float currentHealth;
        private float currentStamina;
        private int currentCoins;

        public int CurrentCoins => currentCoins;
        public bool CanSprint => currentStamina > 0;

        void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadPlayerState(this);
            }
            else
            {
                ResetLocalStats();
            }

            UpdateSliders();
            UpdateCoinUI();
        }

        void Update()
        {
            HandleStamina();
        }

        private void ResetLocalStats()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            currentCoins = 0;
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

            UpdateSliders();
            SaveState();
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0) currentHealth = 0;

            UpdateSliders();
            SaveState();

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void AddCoins(int amount)
        {
            currentCoins += amount;
            UpdateCoinUI();
            SaveState();
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

        public void Win()  // ARA PUBLIC
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

        private void UpdateSliders()
        {
            if (healthSlider != null)
                healthSlider.value = currentHealth;

            if (staminaSlider != null)
                staminaSlider.value = currentStamina;
        }

        private void UpdateCoinUI()
        {
            if (coinText != null)
                coinText.text = "Monedes: " + currentCoins;
        }

        private void SaveState()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SavePlayerState(currentHealth, currentStamina, currentCoins);
            }
        }

        public void SetValues(float health, float stamina, int coins)
        {
            currentHealth = health;
            currentStamina = stamina;
            currentCoins = coins;

            UpdateSliders();
            UpdateCoinUI();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (gameOverSceneAsset != null)
                gameOverSceneName = gameOverSceneAsset.name;

            if (youWinSceneAsset != null)
                youWinSceneName = youWinSceneAsset.name;
        }
#endif
    }
}
