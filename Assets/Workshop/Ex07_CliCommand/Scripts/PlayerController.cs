using UnityEngine;

namespace Workshop.Ex07
{
    /// <summary>
    /// Moves the player with WASD/arrow keys in the editor and tap-to-move on device.
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 6f;
        public float arenaHalfSize = 9f;

        Vector3 _target;
        bool _hasTarget;

        void Update()
        {
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            if (input.sqrMagnitude > 0.01f)
            {
                _hasTarget = false;
                Move(input.normalized);
                return;
            }

            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane ground = new Plane(Vector3.up, Vector3.zero);
                if (ground.Raycast(ray, out float dist))
                {
                    _target = ray.GetPoint(dist);
                    _hasTarget = true;
                }
            }

            if (_hasTarget)
            {
                Vector3 to = _target - transform.position;
                to.y = 0f;
                if (to.magnitude < 0.15f) { _hasTarget = false; return; }
                Move(to.normalized);
            }
        }

        void Move(Vector3 dir)
        {
            Vector3 pos = transform.position + dir * moveSpeed * Time.deltaTime;
            pos.x = Mathf.Clamp(pos.x, -arenaHalfSize, arenaHalfSize);
            pos.z = Mathf.Clamp(pos.z, -arenaHalfSize, arenaHalfSize);
            transform.position = pos;
            if (dir.sqrMagnitude > 0.001f)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 12f * Time.deltaTime);
        }
    }
}
