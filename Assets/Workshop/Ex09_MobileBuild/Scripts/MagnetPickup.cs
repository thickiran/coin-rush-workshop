using UnityEngine;

namespace Workshop.Ex09
{
    /// <summary>
    /// Power-up: grants the player a coin magnet for a few seconds.
    /// </summary>
    public class MagnetPickup : MonoBehaviour
    {
        public float duration = 5f;
        public float spinSpeed = 90f;

        void Update()
        {
            transform.Rotate(0f, spinSpeed * Time.deltaTime, 0f, Space.World);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var magnet = other.GetComponent<PlayerMagnet>();
            if (magnet != null) magnet.Activate(duration);
            Destroy(gameObject);
        }
    }
}
