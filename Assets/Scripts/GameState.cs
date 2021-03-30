using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameState : MonoBehaviour
    {
        public Text ScoreText;
        public Text LivesText;
        public int Lives;

        public bool IsBonusTime => _bonusTime > 0f;
        public bool IsDead => Lives == 0;

        private int _score;
        private float _bonusTime;

        public static GameState GetGameState(GameObject gameObject)
        {
            return gameObject.scene.GetRootGameObjects().First(o => o.name == "Canvas").GetComponent<GameState>();
        }

        private void Start()
        {
            UpdateLives(0);
        }

        private void Update()
        {
            if (_bonusTime > 0f)
            {
                _bonusTime -= Time.deltaTime;
            }
        }

        public void UpdateScore(int delta)
        {
            _score += delta;
            ScoreText.text = "Score: " + _score.ToString().PadLeft(3);
        }

        public void UpdateLives(int delta)
        {
            Lives += delta;
            LivesText.text = "Lives: " + Lives;
        }

        public void BonusTimeStart()
        {
            _bonusTime = 3;
        }
    }
}
