using UnityEngine;

namespace Assets.Scripts.Ghost
{
    public class Move : MonoBehaviour
    {
        public float Speed = 0.2f;
        public Transform[] Waypoints;

        private int _curr;

        private void FixedUpdate()
        {
            var p = Vector2.MoveTowards(transform.position, Waypoints[_curr].position, Speed);
            GetComponent<Rigidbody2D>().MovePosition(p);

            if (transform.position == Waypoints[_curr].position)
            {
                _curr = (_curr + 1) % Waypoints.Length;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "PacMan")
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
