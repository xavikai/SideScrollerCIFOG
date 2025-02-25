using UnityEngine;
using StarterAssets;  // Assegura't que el namespace coincideix amb on es troba el PlayerUIController

public class DamageDealer : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("Quantitat de dany que aquest objecte aplica al tocar-lo")]
    public float damageAmount = 10f;

    [Header("Movement Settings")]
    [Tooltip("Activa el moviment horitzontal")]
    public bool enableHorizontalMovement = false;
    [Tooltip("Activa el moviment vertical")]
    public bool enableVerticalMovement = false;

    [Header("Horizontal Movement Range")]
    [Tooltip("Valor mínim de la posició horitzontal")]
    public float horizontalMin = -5f;
    [Tooltip("Valor màxim de la posició horitzontal")]
    public float horizontalMax = 5f;
    [Tooltip("Velocitat del moviment horitzontal")]
    public float horizontalSpeed = 1f;

    [Header("Vertical Movement Range")]
    [Tooltip("Valor mínim de la posició vertical")]
    public float verticalMin = 0f;
    [Tooltip("Valor màxim de la posició vertical")]
    public float verticalMax = 5f;
    [Tooltip("Velocitat del moviment vertical")]
    public float verticalSpeed = 1f;

    [Header("Invert Movement")]
    [Tooltip("Si està activat, inverteix el moviment horitzontal")]
    public bool invertHorizontalMovement = false;
    [Tooltip("Si està activat, inverteix el moviment vertical")]
    public bool invertVerticalMovement = false;

    [Header("Destruction Settings")]
    [Tooltip("Si és cert, l'objecte es destrueix al tocar el jugador")]
    public bool destroyOnCollision = true;

    // Emmagatzema la posició inicial (opcional)
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        Vector3 newPosition = transform.position;

        // Moviment horitzontal
        if (enableHorizontalMovement)
        {
            float t = Mathf.PingPong(Time.time * horizontalSpeed, 1f);
            if (invertHorizontalMovement)
            {
                newPosition.x = Mathf.Lerp(horizontalMax, horizontalMin, t);
            }
            else
            {
                newPosition.x = Mathf.Lerp(horizontalMin, horizontalMax, t);
            }
        }

        // Moviment vertical
        if (enableVerticalMovement)
        {
            float t = Mathf.PingPong(Time.time * verticalSpeed, 1f);
            if (invertVerticalMovement)
            {
                newPosition.y = Mathf.Lerp(verticalMax, verticalMin, t);
            }
            else
            {
                newPosition.y = Mathf.Lerp(verticalMin, verticalMax, t);
            }
        }

        transform.position = newPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprova que l'objecte que ha tocat té la tag "Player"
        if (other.CompareTag("Player"))
        {
            // Busquem el component PlayerUIController entre els fills del GameObject que ha tocat
            PlayerUIController uiController = other.GetComponentInChildren<PlayerUIController>();

            // Si no es troba, intentem buscar-lo a tota l'escena amb la nova API
            if (uiController == null)
            {
                uiController = Object.FindAnyObjectByType<PlayerUIController>();
            }

            if (uiController != null)
            {
                uiController.TakeDamage(damageAmount);
            }
            else
            {
                Debug.LogWarning("No s'ha trobat cap component PlayerUIController per gestionar la salut.");
            }

            // Si està activada la destrucció, destrueix l'objecte
            if (destroyOnCollision)
            {
                Destroy(gameObject);
            }
        }
    }
}
