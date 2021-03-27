using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameState : MonoBehaviour
    {
        public Text ScoreText;

        public bool IsBonusTime => _bonusTime > 0;

        private int _score;
        private float _bonusTime;

        private void Update()
        {
            if (_bonusTime > 0)
            {
                _bonusTime -= Time.deltaTime;
            }
        }

        public void UpdateScore(int delta)
        {
            _score += delta;
            ScoreText.text = _score.ToString();
        }

        public void BonusTimeStart()
        {
            _bonusTime = 5;
        }
    }
}
