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

    private int currentShots = 0;
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool goingToTarget = true;
    private bool isWaiting = false;
    private float lifeTimer;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        lifeTimer = lifetime;

        if (moveBetweenPoints)
        {
            StartCoroutine(MovingRoutine());
        }
    }

    private void Update()
    {
        if (isProjectile)
        {
            MoveAsProjectile();
        }
    }

    private IEnumerator MovingRoutine()
    {
        Vector3 endPosition = targetPosition;

        while (true)
        {
            if (!isWaiting)
            {
                Vector3 destination = goingToTarget ? endPosition : startPosition;
                transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, destination) < 0.01f)
                {
                    isWaiting = true;
                    yield return new WaitForSeconds(waitTime);
                    goingToTarget = !goingToTarget;
                    isWaiting = false;
                }
            }

            yield return null;
        }
    }

    private void MoveAsProjectile()
    {
        transform.Translate(projectileDirection.normalized * projectileSpeed * Time.deltaTime, Space.World);

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            currentShots++;

            if (repeatProjectile && (maxShots == 0 || currentShots < maxShots))
            {
                ResetProjectile();
            }
            else
            {
                Debug.Log($"💥 Final de projectils ➜ {currentShots} dispar(s)");
                Destroy(gameObject);
            }
        }
    }

    private void ResetProjectile()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        lifeTimer = lifetime;
        Debug.Log($"🔄 Projectil reiniciat ➜ Tir número {currentShots}" + (maxShots > 0 ? $" / {maxShots}" : " (infinit)"));
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
                Debug.Log($"💥 Final de projectils ➜ {currentShots} dispar(s)");
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
    }
}
