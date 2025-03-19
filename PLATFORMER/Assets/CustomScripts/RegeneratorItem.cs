using UnityEngine;

public class RegeneratorItem : MonoBehaviour
{
    [Header("Regeneració Activada")]
    public bool regeneratesHealth = true;
    public bool regeneratesStamina = false;

    [Header("Quantitat a regenerar")]
    public float healthAmount = 20f;
    public float staminaAmount = 20f;

    [Header("Regeneració després de recollir")]
    public bool canRespawn = false;
    public float respawnDelay = 5f;

    [Header("Efectes visuals i sons")]
    public ParticleSystem pickupEffect;
    public AudioClip pickupSound;
    public float soundVolume = 1.0f;

    [Header("Floating Text Configuració")]
    public GameObject floatingTextPrefab;
    public Transform floatingTextSpawnPoint;
    public Color floatingTextColor = Color.green;

    private Renderer[] renderers;
    private Collider[] colliders;
    private AudioSource audioSource;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerStateManager player = PlayerStateManager.Instance;
        bool didSomething = false;

        if (regeneratesHealth && player.currentHealth < player.maxHealth)
        {
            float prevHealth = player.currentHealth;
            player.currentHealth = Mathf.Min(player.currentHealth + healthAmount, player.maxHealth);
            float gained = player.currentHealth - prevHealth;

            if (gained > 0)
            {
                Debug.Log($"❤️ Vida regenerada: {player.currentHealth} / {player.maxHealth} (+{gained})");
                ShowFloatingText($"+{gained} HP");
                didSomething = true;
            }
        }

        if (regeneratesStamina && player.currentStamina < player.maxStamina)
        {
            float prevStamina = player.currentStamina;
            player.currentStamina = Mathf.Min(player.currentStamina + staminaAmount, player.maxStamina);
            float gained = player.currentStamina - prevStamina;

            if (gained > 0)
            {
                Debug.Log($"⚡ Estamina regenerada: {player.currentStamina} / {player.maxStamina} (+{gained})");
                ShowFloatingText($"+{gained} STA");
                didSomething = true;
            }
        }

        if (didSomething)
        {
            PlayPickupEffect();
            PlayPickupSound();

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

    private void PlayPickupSound()
    {
        if (pickupSound != null)
        {
            audioSource.PlayOneShot(pickupSound, soundVolume);
        }
    }

    private void ShowFloatingText(string text)
    {
        if (floatingTextPrefab == null)
        {
            Debug.LogError("❌ No s'ha assignat el prefab de FloatingText al RegeneratorItem!");
            return;
        }

        Transform spawnPoint = floatingTextSpawnPoint != null ? floatingTextSpawnPoint : transform;

        GameObject textObj = Instantiate(floatingTextPrefab, spawnPoint.position, Quaternion.identity);

        if (textObj == null)
        {
            Debug.LogError("❌ No s'ha pogut instanciar el FloatingText!");
            return;
        }

        FloatingText floatingTextScript = textObj.GetComponent<FloatingText>();

        if (floatingTextScript == null)
        {
            Debug.LogError("❌ El prefab no té el script FloatingText assignat!");
            return;
        }

        floatingTextScript.SetupText(text, floatingTextColor);

        Debug.Log($"✅ FloatingText creat ➜ Text: {text} | Vertex Color: {floatingTextColor}");
    }

    private System.Collections.IEnumerator RespawnRoutine()
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
