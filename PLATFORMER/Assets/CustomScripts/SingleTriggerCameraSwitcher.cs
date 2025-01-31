using UnityEngine;
using Cinemachine;

public class SingleTriggerCameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera camera1; // La primera c�mara virtual
    public CinemachineVirtualCamera camera2; // La segunda c�mara virtual
    private CinemachineVirtualCamera activeCamera;

    void Start()
    {
        // Inicialmente, activa la primera c�mara (o la que prefieras)
        ActivateCamera(camera1);
        activeCamera = camera1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Aseg�rate de que tu personaje tenga la etiqueta "Player"
        {
            SwitchCameras();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Aqu� puedes agregar l�gica adicional si es necesario al salir del trigger
        }
    }

    private void SwitchCameras()
    {
        if (activeCamera == camera1)
        {
            ActivateCamera(camera2);
            activeCamera = camera2;
        }
        else if (activeCamera == camera2)
        {
            ActivateCamera(camera1);
            activeCamera = camera1;
        }
    }

    private void ActivateCamera(CinemachineVirtualCamera camera)
    {
        if (camera != null)
        {
            DeactivateAllCameras();
            camera.Priority = 10; // Prioridad alta para activar la c�mara
        }
    }

    private void DeactivateAllCameras()
    {
        if (camera1 != null)
        {
            camera1.Priority = 0; // Prioridad baja para desactivar la c�mara 1
        }
        if (camera2 != null)
        {
            camera2.Priority = 0; // Prioridad baja para desactivar la c�mara 2
        }
    }
}