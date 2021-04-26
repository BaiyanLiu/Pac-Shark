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
        private Vector2 _dir = Vector2.right;

        protected override void OnStart()
        {
            base.OnStart();

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
                _flashTime -= Time.deltaTime;
                if (_flashTime <= 0f)
                {
                    _renderer.enabled = !_renderer.enabled;
                    _flashTime = 0.1f;
                }
            }
            else
            {
                _renderer.enabled = true;
            }
        }

        private void FixedUpdate()
        {
            if (GameState.IsPaused)
            {
                return;
            }

            var p = Vector2.MoveTowards(transform.position, Dest, _gameState.IsDead ? 0.02f : GameState.PacManSpeed);
            Rigidbody.MovePosition(p);

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
                    if (touch.phase == TouchPhase.Began)
                    {
                        var touchPos = Camera.main.ScreenToWorldPoint(new Vector2(touch.position.x, touch.position.y));
                        var touchDir = touchPos - transform.position;
                        horizontal = touchDir.x;
                        vertical = touchDir.y;
                    } 
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        horizontal = touch.deltaPosition.x;
                        vertical = touch.deltaPosition.y;
                    }

                    if (Math.Abs(horizontal) > Math.Abs(vertical))
                    {
                        vertical = 0f;
                    }
                    else
                    {
                        horizontal = 0f;
                    }
                }

                if (vertical > 0.9f && IsValid(Vector2.up, 30))
                {
                    _dir = Vector2.up;
                }
                else if (horizontal > 0.9f && IsValid(Vector2.right, 30))
                {
                    _dir = Vector2.right;
                }
                else if (vertical < -0.9f && IsValid(Vector2.down, 30))
                {
                    _dir = Vector2.down;
                }
                else if (horizontal < -0.9f && IsValid(Vector2.left, 30))
                {
                    _dir = Vector2.left;
                }

                if (IsValid(_dir, 30))
                {
                    Dest = (Vector2) transform.position + _dir;
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

            if (collision.name.StartsWith("Ghost"))
            {
                var ghost = collision.gameObject.GetComponent<Ghost.Ghost>();
                if (ghost.IsDead)
                {
                    return;
                }

                if (_gameState.IsBonusTime)
                {
                    _gameState.GhostEaten();
                    ghost.Die();
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
