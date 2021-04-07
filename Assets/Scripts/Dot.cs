using Assets.Scripts.Scenes;
using UnityEngine;

namespace Assets.Scripts
{
    public class Dot : MonoBehaviour
    {
        public Sprite SmallSprite;
        public Sprite BigSprite;

        private GameState _gameState;
        private bool _isBonus;

        void Start()
        {
            _gameState = GameState.GetGameState(gameObject);
            Reset();

            _gameState.OnLevelChanged += (sender, args) =>
            {
                gameObject.SetActive(true);
                Reset();
            };
        }

        private void Reset()
        {
            _isBonus = Random.value < (0.05f * PlayerPrefs.GetInt(Settings.BonusDots) / 100f);
            GetComponent<SpriteRenderer>().sprite = _isBonus ? BigSprite : SmallSprite;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "PacMan")
            {
                gameObject.SetActive(false);
                _gameState.DotEaten();
                if (_isBonus)
                {
                    _gameState.BonusTimeStart();
                }
            }
        }
    }
}
