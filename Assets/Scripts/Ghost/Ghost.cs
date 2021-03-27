using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Ghost
{
    public class Ghost : MonoBehaviour
    {
        public float Speed = 0.2f;
        public Transform[] Waypoints;

        private GameState _gameState;

        private SpriteRenderer _renderer;
        private Color _originalColor;
        private readonly Color _bonusColor = new Color(0f, 162f / 255f, 232f / 255f);

        private int _currWaypoint;

        private void Start()
        {
            _gameState = gameObject.scene.GetRootGameObjects().First(o => o.name == "Canvas").GetComponent<GameState>();
            _renderer = gameObject.GetComponent<SpriteRenderer>();
            _originalColor = _renderer.color;
        }

        private void Update()
        {
            _renderer.color = _gameState.IsBonusTime ? _bonusColor : _originalColor;
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
