using UnityEngine;
using Color = UnityEngine.Color;

namespace Assets.Scripts.Ghost
{
    public abstract class Ghost : Moveable
    {
        public Color AltColor;

        public bool IsDead { get; private set; }

        protected GameState GameState;
        protected Vector2 OriginalPosition;
        
        private SpriteRenderer _renderer;
        private Color _originalColor;
        private float _speed;

        private void Start()
        {
            GameState = GameState.GetGameState(gameObject);
            OriginalPosition = transform.position;
            _renderer = gameObject.GetComponent<SpriteRenderer>();
            _originalColor = _renderer.color;

            _speed = GameState.GhostSpeed;
            GameState.OnLevelChanged += (sender, args) =>
            {
                _speed = GameState.GhostSpeed;
                transform.position = OriginalPosition;
                OnLevelChanged();
            };

            OnStart();
        }

        protected virtual void OnStart() {}

        protected virtual void OnLevelChanged() {}

        private void Update()
        {
            _renderer.color = GameState.IsBonusTime || IsDead ? AltColor : _originalColor;
        }

        private void FixedUpdate()
        {
            if (GameState.IsPaused)
            {
                return;
            }

            var p = Vector2.MoveTowards(transform.position, IsDead ? OriginalPosition : NextPos, _speed);
            GetComponent<Rigidbody2D>().MovePosition(p);

            if (!GameState.IsBonusTime && IsDead && p == OriginalPosition)
            {
                IsDead = false;
                _speed *= 2;
            }
            else if (!IsDead)
            {
                UpdateNextPos();
            }
        }

        protected abstract Vector2 NextPos { get; }

        protected abstract void UpdateNextPos();

        public void Die()
        {
            IsDead = true;
            _speed /= 2;
            OnDie();
        }

        public virtual void OnDie() {}
    }
}
