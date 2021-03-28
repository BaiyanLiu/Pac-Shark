using UnityEngine;

namespace Assets.Scripts.Ghost
{
    public class Ghost : MonoBehaviour
    {
        public float Speed = 0.2f;
        public Transform[] Waypoints;
        public Color BonusColor;

        private GameState _gameState;
        private SpriteRenderer _renderer;
        private Color _originalColor;

        private int _currWaypoint;

        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);
            _renderer = gameObject.GetComponent<SpriteRenderer>();
            _originalColor = _renderer.color;
        }

        private void Update()
        {
            _renderer.color = _gameState.IsBonusTime ? BonusColor : _originalColor;
        }

        private void FixedUpdate()
        {
            var p = Vector2.MoveTowards(transform.position, Waypoints[_currWaypoint].position, Speed);
            GetComponent<Rigidbody2D>().MovePosition(p);

            if (transform.position == Waypoints[_currWaypoint].position)
            {
                _currWaypoint = (_currWaypoint + 1) % Waypoints.Length;
            }
        }
    }
}
