using UnityEngine;

namespace Assets.Scripts.PacMan
{
    public class Move : MonoBehaviour
    {
        public float Speed = 0.4f;
        public Vector2 Dest = Vector2.zero;

        private bool _isDead;

        private void Start()
        {
            Dest = transform.position;
        }

        private void FixedUpdate()
        {
            if (_isDead)
            {
                return;
            }

            var p = Vector2.MoveTowards(transform.position, Dest, Speed);
            GetComponent<Rigidbody2D>().MovePosition(p);

            if ((Vector2) transform.position == Dest)
            {
                if (Input.GetKey(KeyCode.UpArrow) && IsValid(Vector2.up))
                {
                    Dest = (Vector2) transform.position + Vector2.up;
                }
                if (Input.GetKey(KeyCode.RightArrow) && IsValid(Vector2.right))
                {
                    Dest = (Vector2) transform.position + Vector2.right;
                }
                if (Input.GetKey(KeyCode.DownArrow) && IsValid(Vector2.down))
                {
                    Dest = (Vector2) transform.position + Vector2.down;
                }
                if (Input.GetKey(KeyCode.LeftArrow) && IsValid(Vector2.left))
                {
                    Dest = (Vector2) transform.position + Vector2.left;
                }
            }

            var dir = Dest - (Vector2)transform.position;
            GetComponent<Animator>().SetFloat("DirX", dir.x);
            GetComponent<Animator>().SetFloat("DirY", dir.y);
        }

        private bool IsValid(Vector2 dir)
        {
            Vector2 pos = transform.position;
            var hit = Physics2D.CircleCast(pos, GetComponent<CircleCollider2D>().radius, dir, 1f, 1 << 31);
            return hit.collider == null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "Ghost")
            {
                GetComponent<Animator>().SetBool("Dead", true);
                _isDead = true;
            }
            else if (collision.name == "Tunnel Left")
            {
                transform.position = new Vector2(17f, 0f);
                Dest = (Vector2) transform.position + Vector2.left;
            }
            else if (collision.name == "Tunnel Right")
            {
                transform.position = new Vector2(-16f, 0f);
                Dest = (Vector2)transform.position + Vector2.right;
            }
        }
    }
}
