using UnityEngine;

namespace Assets.Scripts.PacMan
{
    public class Move : MonoBehaviour
    {
        public float Speed = 0.4f;
        public Vector2 Dest = Vector2.zero;

        private GameState _gameState;

        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);
            Dest = transform.position;
        }

        private void FixedUpdate()
        {
            if (_gameState.IsDead)
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

            var dir = Dest - (Vector2) transform.position;
            GetComponent<Animator>().SetFloat("DirX", dir.x);
            GetComponent<Animator>().SetFloat("DirY", dir.y);
        }

        private bool IsValid(Vector2 dir)
        {
            var hit = Physics2D.CircleCast(transform.position, GetComponent<CircleCollider2D>().radius, dir, 1f, 1 << 31);
            return hit.collider == null;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "Ghost")
            {
                if (_gameState.IsBonusTime)
                {
                    Destroy(collision.gameObject);
                }
                else
                {
                    _gameState.UpdateLives(-1);
                    if (_gameState.IsDead)
                    {
                        GetComponent<Animator>().SetBool("Dead", true);
                    }
                }
            }
            else if (collision.name == "Tunnel Left")
            {
                transform.position = new Vector2(17f, 0f);
                Dest = (Vector2) transform.position + Vector2.left;
            }
            else if (collision.name == "Tunnel Right")
            {
                transform.position = new Vector2(-16f, 0f);
                Dest = (Vector2) transform.position + Vector2.right;
            }
        }
    }
}
