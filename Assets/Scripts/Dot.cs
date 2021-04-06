using Assets.Scripts.Scenes;
using UnityEngine;

namespace Assets.Scripts
{
    public class Dot : MonoBehaviour
    {
        public Sprite BigSprite;

        private GameState _gameState;
        private bool _isBonus;

        void Start()
        {
            _gameState = GameState.GetGameState(gameObject);
            _isBonus = Random.value < (0.05f * PlayerPrefs.GetInt(Settings.BonusDots) / 100f);
            if (_isBonus)
            {
                GetComponent<SpriteRenderer>().sprite = BigSprite;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "PacMan")
            {
                Destroy(gameObject);
                _gameState.DotEaten();
                if (_isBonus)
                {
                    _gameState.BonusTimeStart();
                }
            }
        }
    }
}
