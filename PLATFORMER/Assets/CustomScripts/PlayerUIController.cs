using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StarterAssets
{
    public class PlayerUIController : MonoBehaviour
    {
        // Elements de la UI, assignats des de l'Inspector
        public Slider healthSlider;
        public Slider staminaSlider;
        public TMP_Text coinText;

        // Valors m�xims
        public float maxHealth = 100f;
        public float maxStamina = 100f;

        // Variables p�bliques per controlar el drenatge i regeneraci� de l'estamina
        public float runStaminaDrainRate = 10f;  // Estamina perduda per segon mentre sprintes
        public float staminaRegenRate = 5f;      // Estamina regenerada per segon quan no sprintes

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
            // Drena l'estamina quan es mant� apretat Shift, i la regenera quan no
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
    }
}
