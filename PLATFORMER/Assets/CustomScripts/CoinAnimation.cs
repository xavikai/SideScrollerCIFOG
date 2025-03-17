using UnityEngine;

namespace StarterAssets
{
    public class CoinAnimation : MonoBehaviour
    {
        [Header("Rotació")]
        public bool rotate = true;
        public Vector3 rotationAxis = Vector3.up; // Normalment Vector3.up per girar verticalment
        public float rotationSpeed = 90f;         // Graus per segon

        [Header("Oscil·lació Vertical")]
        public bool oscillate = false;
        public float oscillationAmplitude = 0.5f; // Alçada del moviment amunt i avall
        public float oscillationSpeed = 2f;       // Velocitat del moviment

        private Vector3 startPosition;
        private float oscillationTimer = 0f;

        void Start()
        {
            startPosition = transform.position;
        }

        void Update()
        {
            if (rotate)
            {
                transform.Rotate(rotationAxis.normalized, rotationSpeed * Time.deltaTime);
            }

            if (oscillate)
            {
                oscillationTimer += Time.deltaTime * oscillationSpeed;
                float newY = Mathf.Sin(oscillationTimer) * oscillationAmplitude;
                Vector3 offset = new Vector3(0, newY, 0);
                transform.position = startPosition + offset;
            }
        }
    }
}
