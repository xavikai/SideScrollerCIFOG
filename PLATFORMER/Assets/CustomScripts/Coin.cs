using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;
    public float rotationSpeed = 50f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStateManager.Instance.AddCoins(coinValue);
            Destroy(gameObject);
        }
    }
}
