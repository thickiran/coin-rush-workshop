using UnityEngine;

namespace Workshop.Complete
{
    /// <summary>
    /// Patrols between two points and ends the game on contact with the player.
    /// </summary>
    public class Hazard : MonoBehaviour
    {
        public Vector3 pointA = new Vector3(-6f, 0.6f, -3.5f);
        public Vector3 pointB = new Vector3(6f, 0.6f, -3.5f);
        public float speed = 4f;

        void Update()
        {
            float length = Vector3.Distance(pointA, pointB);
            if (length < 0.01f) return;
            float t = Mathf.PingPong(Time.time * speed / length, 1f);
            transform.position = Vector3.Lerp(pointA, pointB, t);
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var gm = FindFirstObjectByType<GameManager>();
            if (gm != null) gm.Lose();
        }
    }
}
