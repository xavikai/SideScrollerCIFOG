using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    [Header("Destí de la plataforma")]
    public Vector3 targetPosition;  // Punt final en coordenades globals

    [Header("Velocitat i Temps d'espera")]
    public float speed = 2.0f;
    public float waitTime = 1.0f;   // Temps d'espera als extrems

    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool goingToTarget = true;
    private bool isWaiting = false; // Controla si estem esperant

    void Start()
    {
        startPosition = transform.position;
        endPosition = targetPosition;
    }

    void Update()
    {
        if (!isWaiting)
        {
            MovePlatform();
        }
    }

    void MovePlatform()
    {
        Vector3 destination = goingToTarget ? endPosition : startPosition;

        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.deltaTime);

        // Ha arribat al punt de destí?
        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;

        // Espera el temps definit abans de canviar direcció
        yield return new WaitForSeconds(waitTime);

        goingToTarget = !goingToTarget;
        isWaiting = false;
    }

    // Opcional: Dibuixar a l'editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, targetPosition);
        Gizmos.DrawSphere(targetPosition, 0.2f);
    }
}
