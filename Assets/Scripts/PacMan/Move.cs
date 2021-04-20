using System;
using UnityEngine;

namespace Assets.Scripts.PacMan
{
    public class Move : Moveable
    {
        public Vector2 Dest { get; private set; }

        private GameState _gameState;
        private SpriteRenderer _renderer;
        private Animator _animator;
        private Vector2 _originalPosition;
        private float _invincibleTime;
        private float _flashTime;

        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);
            _renderer = GetComponent<SpriteRenderer>();
            _animator = GetComponent<Animator>();
            _originalPosition = transform.position;
            Reset();

            _gameState.OnLevelChanged += (sender, args) =>
            {
                Reset();
            };
            _gameState.OnDie += (sender, args) =>
            {
                Dest = new Vector2(transform.position.x, GameState.Max.y + 1);
            };
        }

        private void Reset()
        {
            Dest =  transform.position = _originalPosition;
        }

        private void Update()
        {
            if (GameState.IsPaused)
            {
                return;
            }

            if (_invincibleTime > 0f)
            {
                _invincibleTime -= Time.deltaTime;
            }
            if (_flashTime > 0f)
            {
                _flashTime -= Time.deltaTime;
            }

            if (_flashTime <= 0f)
            {
                if (_invincibleTime > 0f)
                {
                    _renderer.enabled = !_renderer.enabled;
                }
                else
                {
                    _renderer.enabled = true;
                }
                _flashTime = 0.1f;
            }
        }

        private void FixedUpdate()
        {
            if (GameState.IsPaused)
            {
                return;
            }

            var p = Vector2.MoveTowards(transform.position, Dest, _gameState.IsDead ? 0.02f : GameState.PacManSpeed);
            GetComponent<Rigidbody2D>().MovePosition(p);

            if (_gameState.IsDead)
            {
                return;
            }

            if ((Vector2) transform.position == Dest)
            {
                var horizontal = Input.GetAxisRaw("Horizontal");
                var vertical = Input.GetAxisRaw("Vertical");

                if (Input.touchCount > 0)
                {
                    var touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Stationary || touch.phase == TouchPhase.Moved)
                    {
                        var touchPos = Camera.main.ScreenToWorldPoint(new Vector2(touch.position.x, touch.position.y));
                        var touchDelta = touchPos - transform.position;
                        horizontal = touchDelta.x;
                        vertical = touchDelta.y;
                    }
                }

                if (vertical > 0.1f && IsValid(Vector2.up, 30))
                {
                    Dest = (Vector2) transform.position + Vector2.up;
                }
                if (horizontal > 0.1f && IsValid(Vector2.right, 30))
                {
                    Dest = (Vector2) transform.position + Vector2.right;
                }
                if (vertical < -0.1f && IsValid(Vector2.down, 30))
                {
                    Dest = (Vector2) transform.position + Vector2.down;
                }
                if (horizontal < -0.1f && IsValid(Vector2.left, 30))
                {
                    Dest = (Vector2) transform.position + Vector2.left;
                }
            }

            var dir = Dest - (Vector2) transform.position;
            _animator.SetFloat("DirX", dir.x);
            _animator.SetFloat("DirY", dir.y);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_gameState.IsDead)
            {
                return;
            }

            if (collision.name.StartsWith("Ghost") && !collision.gameObject.GetComponent<Ghost.Ghost>().IsDead)
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
                        _animator.SetBool("Dead", true);
                    }
                    else
                    {
                        _invincibleTime = 3f;
                    }
                }
            }
            else if (collision.name.StartsWith("Tunnel"))
            {
                if (collision.name.Contains("Left"))
                {
                    transform.position = new Vector2(18f, transform.position.y);
                    Dest = (Vector2) transform.position + Vector2.left;
                }
                else
                {
                    transform.position = new Vector2(-18f, transform.position.y);
                    Dest = (Vector2) transform.position + Vector2.right;
                }
            }
        }
    }
}
