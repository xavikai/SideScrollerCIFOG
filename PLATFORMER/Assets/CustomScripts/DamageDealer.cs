using UnityEngine;
using System.Collections;

public class DamageDealer : MonoBehaviour
{
    [Header("Dany")]
    public float damageAmount = 10f;

    [Header("Tipus de moviment")]
    public bool moveBetweenPoints = false;
    public Vector3 targetPosition;
    public float moveSpeed = 2f;
    public float waitTime = 1f;

    [Header("Projectil")]
    public bool isProjectile = false;
    public Vector3 projectileDirection = Vector3.forward;
    public float projectileSpeed = 10f;
    public float lifetime = 5f;

    [Header("Repetició Projectil")]
    public bool repeatProjectile = false;
    [Tooltip("0 ➜ dispararà infinitament")]
    public int maxShots = 0;

    [Header("Física (Opcional)")]
    public bool usePhysics = false;
    public float impulseForce = 10f;
    public float projectileMass = 1f;

    private int currentShots = 0;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool goingToTarget = true;
    private bool isWaiting = false;
    private float lifeTimer;

    private Rigidbody rb;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        lifeTimer = lifetime;

        if (moveBetweenPoints)
        {
            StartCoroutine(MovingRoutine());
        }

        if (isProjectile)
        {
            if (usePhysics)
            {
                SetupRigidbody();
                LaunchWithPhysics();
            }
        }
    }

    private void Update()
    {
        if (isProjectile && !usePhysics)
        {
            MoveAsProjectile();
        }

        // Controla el temps de vida, tant si fa servir física com no
        lifeTimer -= Time.deltaTime;

        if (isProjectile && lifeTimer <= 0f)
        {
            currentShots++;

            if (repeatProjectile && (maxShots == 0 || currentShots < maxShots))
            {
                ResetProjectile();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void SetupRigidbody()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.mass = projectileMass;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    private void LaunchWithPhysics()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Aplica una força impulsiva en la direcció especificada
        rb.AddForce(projectileDirection.normalized * impulseForce, ForceMode.Impulse);
    }

    private void ResetProjectile()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        lifeTimer = lifetime;

        if (usePhysics)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            LaunchWithPhysics();
        }

        Debug.Log($"🔄 Projectil reiniciat ➜ Tir número {currentShots}" + (maxShots > 0 ? $" / {maxShots}" : " (infinit)"));
    }

    private void MoveAsProjectile()
    {
        transform.Translate(projectileDirection.normalized * projectileSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateManager.Instance.TakeDamage(damageAmount);
        }

        if (isProjectile)
        {
            currentShots++;

            if (repeatProjectile && (maxShots == 0 || currentShots < maxShots))
            {
                ResetProjectile();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (moveBetweenPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, targetPosition);
            Gizmos.DrawSphere(targetPosition, 0.2f);
        }

        if (isProjectile)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, projectileDirection.normalized * 2f);
        }
    }
}
