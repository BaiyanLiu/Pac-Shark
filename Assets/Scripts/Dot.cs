using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public class Dot : MonoBehaviour
    {
        public Sprite BigSprite;

        private GameState _gameState;

        private bool _isBig;

        void Start()
        {
            _gameState = gameObject.scene.GetRootGameObjects().First(o => o.name == "Canvas").GetComponent<GameState>();
            _isBig = Random.value < 0.05f;
            if (_isBig)
            {
                GetComponent<SpriteRenderer>().sprite = BigSprite;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "PacMan")
            {
                Destroy(gameObject);
                _gameState.UpdateScore(1);
                if (_isBig)
                {
                    _gameState.BonusTimeStart();
                }
            }
        }
    }
}
