using UnityEngine;
using Cinemachine;

public class ZoneBasedCameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera zoneCamera; // La cámara específica para la zona
    public CinemachineVirtualCamera defaultCamera; // La cámara por defecto

    private int playerInZoneCount = 0; // Contador de cuántos triggers están activados
    private CinemachineVirtualCamera activeCamera;

    void Start()
    {
        // Inicialmente, activa la cámara por defecto
        ActivateCamera(defaultCamera);
        activeCamera = defaultCamera;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que tu personaje tenga la etiqueta "Player"
        {
            playerInZoneCount++;
            CheckAndSwitchCamera();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZoneCount--;
            CheckAndSwitchCamera();
        }
    }

    private void CheckAndSwitchCamera()
    {
        if (playerInZoneCount > 0 && activeCamera != zoneCamera)
        {
            SwitchToCamera(zoneCamera);
        }
        else if (playerInZoneCount <= 0 && activeCamera != defaultCamera)
        {
            SwitchToCamera(defaultCamera);
        }
    }

    private void SwitchToCamera(CinemachineVirtualCamera newCamera)
    {
        if (newCamera != null && newCamera != activeCamera)
        {
            DeactivateAllCameras();
            ActivateCamera(newCamera);
            activeCamera = newCamera;
        }
    }

    private void ActivateCamera(CinemachineVirtualCamera camera)
    {
        if (camera != null)
        {
            camera.Priority = 10; // Prioridad alta para activar la cámara
        }
    }

    private void DeactivateAllCameras()
    {
        if (zoneCamera != null)
        {
            zoneCamera.Priority = 0; // Prioridad baja para desactivar la cámara de la zona
        }
        if (defaultCamera != null)
        {
            defaultCamera.Priority = 0; // Prioridad baja para desactivar la cámara por defecto
        }
    }
}