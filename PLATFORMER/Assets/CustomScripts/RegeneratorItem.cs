using UnityEngine;
using System.Collections;

public class RegeneratorItem : MonoBehaviour
{
    [Header("Regeneració Activada")]
    public bool regeneratesHealth = true;
    public bool regeneratesStamina = false;

    [Header("Quantitat a regenerar")]
    public float healthAmount = 20f;
    public float staminaAmount = 20f;

    [Header("Respawn després de recollir")]
    public bool canRespawn = false;
    public float respawnDelay = 5f;

    [Header("Efectes visuals")]
    public ParticleSystem pickupEffect;

    [Header("Floating Text Configuració")]
    public GameObject floatingTextPrefab;
    public Transform floatingTextSpawnPoint;
    public Color floatingTextColor = Color.green;

    private Renderer[] renderers;
    private Collider[] colliders;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStateManager player = PlayerStateManager.Instance;
        bool didSomething = false;

        if (regeneratesHealth && player.currentHealth < player.maxHealth)
        {
            float gained = Mathf.Min(healthAmount, player.maxHealth - player.currentHealth);
            player.currentHealth += gained;

            if (gained > 0)
            {
                ShowFloatingText($"+{gained} HP");
                didSomething = true;
            }
        }

        if (regeneratesStamina && player.currentStamina < player.maxStamina)
        {
            float gained = Mathf.Min(staminaAmount, player.maxStamina - player.currentStamina);
            player.currentStamina += gained;

            if (gained > 0)
            {
                ShowFloatingText($"+{gained} STA");
                didSomething = true;
            }
        }

        if (didSomething)
        {
            PlayPickupEffect();
            // So centralitzat via AudioManager
            AudioManager.Instance?.PlaySound(AudioManager.Instance.regeneratorPickupSound, transform.position);

            if (canRespawn)
            {
                StartCoroutine(RespawnRoutine());
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void PlayPickupEffect()
    {
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }
    }

    private void ShowFloatingText(string text)
    {
        if (floatingTextPrefab == null) return;

        Transform spawnPoint = floatingTextSpawnPoint != null ? floatingTextSpawnPoint : transform;
        GameObject textObj = Instantiate(floatingTextPrefab, spawnPoint.position, Quaternion.identity);

        FloatingText floatingTextScript = textObj.GetComponent<FloatingText>();
        if (floatingTextScript != null)
        {
            floatingTextScript.SetupText(text, floatingTextColor);
        }
    }

    private IEnumerator RespawnRoutine()
    {
        HideObject();
        yield return new WaitForSeconds(respawnDelay);
        ShowObject();
    }

    private void HideObject()
    {
        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }

    private void ShowObject()
    {
        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }
    }
}
