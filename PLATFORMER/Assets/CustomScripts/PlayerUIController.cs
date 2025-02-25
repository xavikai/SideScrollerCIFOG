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
        // Elements de la UI, assignats des de l'Inspector
        public Slider healthSlider;
        public Slider staminaSlider;
        public TMP_Text coinText;

        // Valors màxims
        public float maxHealth = 100f;
        public float maxStamina = 100f;

        // Variables públiques per controlar el drenatge i regeneració de l'estamina
        public float runStaminaDrainRate = 10f;  // Estamina perduda per segon mentre sprintes
        public float staminaRegenRate = 5f;      // Estamina regenerada per segon quan no sprintes

        // Variable per assignar l'asset de la escena Game Over (només visible a l'Editor)
#if UNITY_EDITOR
        public SceneAsset gameOverSceneAsset;
#endif
        // Aquesta cadena es carrega automàticament amb el nom de l'asset GameOver
        [HideInInspector]
        public string gameOverSceneName;

        // Variables internes per als valors actuals
        private float currentHealth;
        private float currentStamina;
        private int currentCoins;

        // Propietat per saber si es pot sprintar (retorna true si encara hi ha estamina)
        public bool CanSprint
        {
            get { return currentStamina > 0; }
        }

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
            // Drena l'estamina quan es manté apretat Shift, i la regenera quan no
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentStamina -= runStaminaDrainRate * Time.deltaTime;
                if (currentStamina < 0)
                    currentStamina = 0;
            }
            else
            {
                currentStamina += staminaRegenRate * Time.deltaTime;
                if (currentStamina > maxStamina)
                    currentStamina = maxStamina;
            }

            if (staminaSlider != null)
            {
                staminaSlider.value = currentStamina;
            }
        }

        public void UpdateHealth(float newHealth)
        {
            currentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
            if (healthSlider != null)
            {
                healthSlider.value = currentHealth;
            }
            Debug.Log("Vida restant: " + currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        // Aquest mètode rep dany i actualitza la salut
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0)
                currentHealth = 0;
            if (healthSlider != null)
            {
                healthSlider.value = currentHealth;
            }
            Debug.Log("Vida restant: " + currentHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void UpdateStamina(float newStamina)
        {
            currentStamina = Mathf.Clamp(newStamina, 0, maxStamina);
            if (staminaSlider != null)
            {
                staminaSlider.value = currentStamina;
            }
        }

        public void AddCoins(int amount)
        {
            currentCoins += amount;
            UpdateCoinUI();
        }

        public void RemoveCoins(int amount)
        {
            currentCoins -= amount;
            if (currentCoins < 0)
                currentCoins = 0;
            UpdateCoinUI();
        }

        private void UpdateCoinUI()
        {
            if (coinText != null)
            {
                coinText.text = "Monedes: " + currentCoins.ToString();
            }
        }

        // Aquesta funció s'executa quan la salut arriba a 0: carrega l'escena Game Over
        private void Die()
        {
            Debug.Log("El jugador ha mort. Carregant Game Over...");
            if (!string.IsNullOrEmpty(gameOverSceneName))
            {
                SceneManager.LoadScene(gameOverSceneName);
            }
            else
            {
                Debug.LogWarning("No s'ha assignat l'asset de Game Over.");
            }
        }

#if UNITY_EDITOR
        // Aquesta funció s'executa a l'Editor quan hi ha canvis a l'Inspector.
        // Actualitza la cadena gameOverSceneName amb el nom de l'asset assignat.
        private void OnValidate()
        {
            if (gameOverSceneAsset != null)
            {
                gameOverSceneName = gameOverSceneAsset.name;
            }
        }
#endif
    }
}
