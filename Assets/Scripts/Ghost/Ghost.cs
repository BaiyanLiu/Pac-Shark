using UnityEngine;

namespace Assets.Scripts.Ghost
{
    public abstract class Ghost : Moveable
    {
        public float Speed = 0.2f;
        public Color AltColor;

        public bool IsDead { get; private set; }

        protected GameState GameState;
        protected Vector3 OriginalPosition;
        
        private SpriteRenderer _renderer;
        private Color _originalColor;

        private void Start()
        {
            GameState = GameState.GetGameState(gameObject);
            OriginalPosition = transform.position;
            _renderer = gameObject.GetComponent<SpriteRenderer>();
            _originalColor = _renderer.color;
            OnStart();
        }

        protected virtual void OnStart() {}

        private void Update()
        {
            _renderer.color = GameState.IsBonusTime || IsDead ? AltColor : _originalColor;
        }

        private void FixedUpdate()
        {
            var p = Vector2.MoveTowards(transform.position, IsDead ? OriginalPosition : NextPos, Speed);
            GetComponent<Rigidbody2D>().MovePosition(p);

            if (!GameState.IsBonusTime && IsDead && p == (Vector2) OriginalPosition)
            {
                IsDead = false;
                Speed *= 2;
            }
            else if (!IsDead)
            {
                UpdateNextPos();
            }
        }

        protected abstract Vector3 NextPos { get; }

        protected abstract void UpdateNextPos();

        public void Die()
        {
            IsDead = true;
            Speed /= 2;
            OnDie();
        }

        public virtual void OnDie() {}
    }
}
