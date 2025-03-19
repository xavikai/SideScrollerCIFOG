using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Configuració de la moneda")]
    public int coinValue = 1;
    public float rotationSpeed = 50f;

    [Header("Efectes visuals i sons")]
    public ParticleSystem pickupEffect;
    public AudioClip pickupSound;
    public float soundVolume = 1.0f;

    private AudioSource audioSource;

    private void Awake()
    {
        // Busquem o afegim l'AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false; // No ha de sonar automàticament
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // ✅ Afegim les monedes al jugador
            PlayerStateManager.Instance.AddCoins(coinValue);

            // ✅ Executem l'efecte de partícules (si hi ha)
            PlayPickupEffect();

            // ✅ Reproduïm el so (si hi ha)
            PlayPickupSound();

            // ✅ Destruïm la moneda després de l'efecte
            Destroy(gameObject);
        }
    }

    private void PlayPickupEffect()
    {
        if (pickupEffect != null)
        {
            // Instanciem les partícules en la posició de la moneda
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }
    }

    private void PlayPickupSound()
    {
        if (pickupSound != null)
        {
            // Reproduïm el clip una vegada en la posició actual
            AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
        }
    }
}
