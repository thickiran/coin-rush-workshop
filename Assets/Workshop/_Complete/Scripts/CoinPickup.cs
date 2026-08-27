using UnityEngine;

namespace Workshop.Complete
{
    /// <summary>
    /// Spins the coin and awards score when the player touches it.
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
            var score = FindFirstObjectByType<ScoreManager>();
            if (score != null) score.AddCoins(value);
            Destroy(gameObject);
        }
    }
}
