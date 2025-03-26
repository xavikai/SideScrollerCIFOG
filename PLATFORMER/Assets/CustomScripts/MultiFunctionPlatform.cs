using UnityEngine;
using System.Collections;

public enum PlatformType
{
    Static,
    Moving,
    Falling,
    Destructible
}

public class MultiFunctionPlatform : MonoBehaviour
{
    [Header("Tipus de plataforma")]
    public PlatformType platformType = PlatformType.Static;

    [Header("Paràmetres de moviment (Moving Platform)")]
    public Vector3 targetPosition;
    public float speed = 2.0f;
    public float waitTime = 1.0f;

    [Header("Paràmetres de plataforma caient (Falling Platform)")]
    public float fallDelay = 1.0f;
    private bool hasFallen = false;

    [Header("Paràmetres de destrucció (Destructible Platform)")]
    public float destroyDelay = 1.0f;

    [Header("Paràmetres de respawn (Falling & Destructible)")]
    public bool respawn = false;
    public float respawnDelay = 3.0f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool goingToTarget = true;
    private bool isWaiting = false;

    private Rigidbody rb;
    private Collider[] colliders;
    private Renderer[] renderers;

    private AudioManager audioManager;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        audioManager = AudioManager.Instance;

        if (platformType == PlatformType.Falling)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }

        colliders = GetComponentsInChildren<Collider>();
        renderers = GetComponentsInChildren<Renderer>();

        if (platformType == PlatformType.Moving)
        {
            StartCoroutine(MovingPlatformRoutine());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        switch (platformType)
        {
            case PlatformType.Falling:
                if (!hasFallen)
                    StartCoroutine(FallAfterDelay());
                break;

            case PlatformType.Destructible:
                StartCoroutine(DestroyAfterDelay());
                break;
        }
    }

    IEnumerator MovingPlatformRoutine()
    {
        Vector3 endPosition = targetPosition;

        while (true)
        {
            if (!isWaiting)
            {
                Vector3 destination = goingToTarget ? endPosition : startPosition;
                transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, destination) < 0.01f)
                {
                    audioManager?.PlaySound(audioManager.platformMoveSound, transform.position);
                    isWaiting = true;
                    yield return new WaitForSeconds(waitTime);
                    goingToTarget = !goingToTarget;
                    isWaiting = false;
                }
            }

            yield return null;
        }
    }

    IEnumerator FallAfterDelay()
    {
        hasFallen = true;
        yield return new WaitForSeconds(fallDelay);

        if (rb != null)
        {
            rb.isKinematic = false;
            audioManager?.PlaySound(audioManager.platformFallSound, transform.position);
        }

        if (respawn)
        {
            yield return new WaitForSeconds(respawnDelay);
            RespawnPlatform();
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);

        HidePlatform();
        audioManager?.PlaySound(audioManager.platformDestroySound, transform.position);

        if (respawn)
        {
            yield return new WaitForSeconds(respawnDelay);
            RespawnPlatform();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void HidePlatform()
    {
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }

        foreach (Renderer rend in renderers)
        {
            rend.enabled = false;
        }
    }

    void RespawnPlatform()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;

        if (platformType == PlatformType.Falling && rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            hasFallen = false;
        }

        foreach (Collider col in colliders)
        {
            col.enabled = true;
        }

        foreach (Renderer rend in renderers)
        {
            rend.enabled = true;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (platformType == PlatformType.Moving)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, targetPosition);
            Gizmos.DrawSphere(targetPosition, 0.2f);
        }
    }
}
