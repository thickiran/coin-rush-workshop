using UnityEngine;

namespace Workshop.Ex04
{
    /// <summary>
    /// Spins the coin and disappears when the player touches it.
    /// </summary>
    public class CoinPickup : MonoBehaviour
    {
        public int value = 1;
        public float spinSpeed = 120f;

        void Update()
        {
            transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            Debug.Log("Coin collected!");
            Destroy(gameObject);
        }
    }
}
