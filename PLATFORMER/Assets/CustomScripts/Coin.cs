using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Configuració de la moneda")]
    public int coinValue = 1;
    public float rotationSpeed = 50f;

    [Header("Efectes visuals")]
    public ParticleSystem pickupEffect;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Afegim monedes al jugador
            PlayerStateManager.Instance.AddCoins(coinValue);

            // Efecte de partícules
            PlayPickupEffect();

            // Reproduir so centralitzat via AudioManager
            AudioManager.Instance?.PlaySound(AudioManager.Instance.coinPickupSound, transform.position);

            // Destruïm la moneda
            Destroy(gameObject);
        }
    }

    private void PlayPickupEffect()
    {
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }
    }
}
