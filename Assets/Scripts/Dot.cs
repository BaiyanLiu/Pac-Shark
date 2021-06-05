using Assets.Scripts.Scenes;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public class Dot : MonoBehaviour
    {
        public bool CanBeBonus;

        private GameState _gameState;
        private bool _isBonus;

        [UsedImplicitly]
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
            _isBonus = CanBeBonus && Random.value < (0.5f * PlayerPrefs.GetInt(Settings.BonusDots) / 100f);
            gameObject.transform.localScale = _isBonus ? new Vector2(0.2f, 0.2f) : new Vector2(0.1f, 0.1f);
        }

        [UsedImplicitly]
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
