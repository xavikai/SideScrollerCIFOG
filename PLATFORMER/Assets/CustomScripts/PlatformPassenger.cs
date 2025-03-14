using UnityEngine;

public class PlatformPassenger : MonoBehaviour
{
    private Transform playerOnPlatform;
    private Vector3 lastPlatformPosition;

    void Start()
    {
        lastPlatformPosition = transform.position;
    }

    void FixedUpdate() // Mou el player segons el moviment de la plataforma
    {
        if (playerOnPlatform != null)
        {
            Vector3 platformMovement = transform.position - lastPlatformPosition;
            playerOnPlatform.transform.position += platformMovement;
        }

        lastPlatformPosition = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnPlatform = other.transform;
            Debug.Log("Jugador enganxat a la plataforma");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerOnPlatform == other.transform)
        {
            playerOnPlatform = null;
            Debug.Log("Jugador desenganxat de la plataforma");
        }
    }
}
