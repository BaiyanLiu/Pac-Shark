using UnityEngine;

namespace Assets.Scripts.PacMan
{
    public class Move : MonoBehaviour
    {
        public float Speed = 0.4f;
        public Vector2 Dest = Vector2.zero;

        private GameState _gameState;
        private SpriteRenderer _renderer;
        private Color _originalColor;
        private float _invincibleTime;
        private float _invincibleFlashTime;

        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);
            _renderer = gameObject.GetComponent<SpriteRenderer>();
            _originalColor = _renderer.color;
            Dest = transform.position;
        }

        private void Update()
        {
            if (_invincibleTime > 0f)
            {
                _invincibleTime -= Time.deltaTime;
            }
            if (_invincibleFlashTime > 0f)
            {
                _invincibleFlashTime -= Time.deltaTime;
            }

            if (_invincibleFlashTime <= 0f)
            {
                if (_invincibleTime > 0f)
                {
                    _renderer.color = _renderer.color == _originalColor ? Color.black : _originalColor;
                }
                else
                {
                    _renderer.color = _originalColor;
                }
                _invincibleFlashTime = 0.1f;
            }
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
                var horizontal = Input.GetAxisRaw("Horizontal");
                var vertical = Input.GetAxisRaw("Vertical");

                if (vertical > 0.1f && IsValid(Vector2.up))
                {
                    Dest = (Vector2) transform.position + Vector2.up;
                }
                if (horizontal > 0.1f && IsValid(Vector2.right))
                {
                    Dest = (Vector2) transform.position + Vector2.right;
                }
                if (vertical < -0.1f && IsValid(Vector2.down))
                {
                    Dest = (Vector2) transform.position + Vector2.down;
                }
                if (horizontal < -0.1f && IsValid(Vector2.left))
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
            if (collision.name == "Ghost" && !collision.gameObject.GetComponent<Ghost.Ghost>().IsDead)
            {
                if (_gameState.IsBonusTime)
                {
                    collision.gameObject.GetComponent<Ghost.Ghost>().Die();
                }
                else if (_invincibleTime <= 0f)
                {
                    _gameState.UpdateLives(-1);
                    if (_gameState.IsDead)
                    {
                        GetComponent<Animator>().SetBool("Dead", true);
                    }
                    else
                    {
                        _invincibleTime = 3f;
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
