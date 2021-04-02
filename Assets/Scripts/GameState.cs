using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameState : MonoBehaviour
    {
        public static readonly Vector2Int Min = new Vector2Int(-13, -13);
        public static readonly Vector2Int Max = new Vector2Int(14, 13);

        public Text ScoreText;
        public Image[] LivesImages;
        public int Lives;

        public bool IsBonusTime => _bonusTime > 0f;
        public bool IsDead => Lives == 0;

        private int _score;
        private float _bonusTime;

        public static GameState GetGameState(GameObject gameObject)
        {
            return gameObject.scene.GetRootGameObjects().First(o => o.name == "Canvas").GetComponent<GameState>();
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
            ScoreText.text = _score.ToString().PadLeft(3, '0');
        }

        public void UpdateLives(int delta)
        {
            Lives += delta;
            for (var i = Lives; i < LivesImages.Length; i++)
            {
                LivesImages[i].color = Color.black;
            }
        }

        public void BonusTimeStart()
        {
            _bonusTime = 3;
        }
    }
}
