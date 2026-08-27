using UnityEngine;

namespace Workshop.Ex09
{
    /// <summary>
    /// While active, pulls nearby coins toward the player.
    /// </summary>
    public class PlayerMagnet : MonoBehaviour
    {
        public float pullRadius = 4f;
        public float pullSpeed = 10f;

        float _activeUntil = -1f;

        public bool IsActive => Time.time <= _activeUntil;

        public void Activate(float duration)
        {
            _activeUntil = Time.time + duration;
        }

        void Update()
        {
            if (!IsActive) return;
            foreach (var coin in FindObjectsByType<CoinPickup>(FindObjectsSortMode.None))
            {
                Vector3 to = transform.position - coin.transform.position;
                if (to.magnitude > pullRadius) continue;
                coin.transform.position = Vector3.MoveTowards(
                    coin.transform.position, transform.position, pullSpeed * Time.deltaTime);
            }
        }
    }
}
