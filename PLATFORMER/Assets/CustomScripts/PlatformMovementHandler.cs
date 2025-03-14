using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlatformMovementHandler : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 platformDelta = Vector3.zero;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void LateUpdate()
    {
        if (platformDelta != Vector3.zero)
        {
            Debug.Log($"Applying platform delta movement: {platformDelta}");
        }

        controller.Move(platformDelta);

        // Netegem el delta després d'aplicar-lo
        platformDelta = Vector3.zero;
    }

    // Aquest mètode el crida la plataforma per passar el moviment
    public void ApplyPlatformDelta(Vector3 delta)
    {
        platformDelta += delta;
    }
}
