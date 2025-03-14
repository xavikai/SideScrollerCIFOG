using UnityEngine;
using System.Collections;

public class PlataformaMovimentRaycast : MonoBehaviour
{
    [Header("Punts de moviment")]
    public Transform puntB; // Punt destí, ho pots moure al scene view.

    [Header("Velocitat de moviment")]
    public float velocitat = 2f;

    [Header("Raycast configuració")]
    public float distanciaRaycast = 1.5f;

    [Header("Temps d'espera a cada punt")]
    public float waitTime = 2f;

    private Vector3 puntA; // Posició inicial (punt de partida)
    private Vector3 target;
    private Vector3 posicioAnterior;
    private PlatformMovementHandler platformHandler = null;

    void Start()
    {
        puntA = transform.position;
        target = puntB.position;
        posicioAnterior = transform.position;

        StartCoroutine(MoureEntrePunts());
    }

    void Update()
    {
        ComprovarSiJugadorSobre();
        posicioAnterior = transform.position;
    }

    IEnumerator MoureEntrePunts()
    {
        while (true)
        {
            while (Vector3.Distance(transform.position, target) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, velocitat * Time.deltaTime);
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
            target = (target == puntA) ? puntB.position : puntA;
        }
    }

    private void ComprovarSiJugadorSobre()
    {
        RaycastHit hit;

        Vector3 origenRaycast = transform.position + Vector3.up * 0.1f;
        Vector3 direccioRaycast = Vector3.up;

        Debug.DrawRay(origenRaycast, direccioRaycast * distanciaRaycast, Color.green);

        if (Physics.Raycast(origenRaycast, direccioRaycast, out hit, distanciaRaycast))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (platformHandler == null)
                {
                    platformHandler = hit.collider.GetComponent<PlatformMovementHandler>();
                }
            }
        }
        else
        {
            platformHandler = null;
        }

        if (platformHandler != null)
        {
            var thirdPersonController = platformHandler.GetComponent<StarterAssets.ThirdPersonController>();

            if (thirdPersonController != null && thirdPersonController.Grounded)
            {
                Vector3 deltaMoviment = transform.position - posicioAnterior;

                // Ajusta el factor de deltaMovement segons el cas
                deltaMoviment *= 1f; // Pots provar amb 0.5f o 2f segons el resultat

                // Debug per veure el moviment transmès al jugador
                Debug.Log($"Delta moviment plataforma -> jugador: {deltaMoviment}");

                platformHandler.ApplyPlatformDelta(deltaMoviment);
            }
        }
    }
}
