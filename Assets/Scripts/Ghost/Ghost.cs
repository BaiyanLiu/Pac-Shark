using UnityEngine;

namespace Assets.Scripts.Ghost
{
    public class Ghost : MonoBehaviour
    {
        public float Speed = 0.2f;
        public Transform[] Waypoints;
        public Color AltColor;

        private GameState _gameState;
        private SpriteRenderer _renderer;
        private Color _originalColor;
        private Vector3 _originalPosition;

        private int _currWaypoint;
        private int _prevWaypoint;
        private bool _isDead;

        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);
            _renderer = gameObject.GetComponent<SpriteRenderer>();
            _originalColor = _renderer.color;
            _originalPosition = transform.position;
        }

        private void Update()
        {
            _renderer.color = _gameState.IsBonusTime || _isDead ? AltColor : _originalColor;
        }

        private void FixedUpdate()
        {
            var p = Vector2.MoveTowards(transform.position, _isDead ? _originalPosition : Waypoints[_gameState.IsBonusTime ? _prevWaypoint : _currWaypoint].position, Speed);
            GetComponent<Rigidbody2D>().MovePosition(p);

            if (!_gameState.IsBonusTime && _isDead && p == (Vector2) _originalPosition)
            {
                _isDead = false;
                Speed *= 2;
            } else if (!_isDead && transform.position == Waypoints[_gameState.IsBonusTime ? _prevWaypoint : _currWaypoint].position)
            {
                _currWaypoint = (_currWaypoint + Waypoints.Length + (_gameState.IsBonusTime ? -1 : 1)) % Waypoints.Length;
                _prevWaypoint = (_currWaypoint + Waypoints.Length - 1) % Waypoints.Length;
            }
        }

        public void Die()
        {
            _isDead = true;
            _currWaypoint = 0;
            Speed /= 2;
        }
    }
}
