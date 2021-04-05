using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameState : MonoBehaviour
    {
        public static readonly Vector2Int Min = new Vector2Int(-15, -13);
        public static readonly Vector2Int Max = new Vector2Int(15, 13);

        public event EventHandler OnLevelChanged;

        public Text ScoreText;
        public Text HighScoreText;
        public Image[] LivesImages;
        public int Lives;
        public GameObject[] Levels;

        public bool IsBonusTime => _bonusTime > 0f;
        public bool IsDead => Lives == 0;

        public const float PacManSpeed = 0.4f;
        public float GhostSpeed { get; private set; } = 0.2f;
        public Transform[] Waypoints { get; private set; }

        private int _score;
        private int _highScore;
        private float _bonusTime;
        private int _level;
        private int _numDots;

        public static GameState GetGameState(GameObject gameObject)
        {
            return gameObject.scene.GetRootGameObjects().First(o => o.name == "Canvas").GetComponent<GameState>();
        }

        private void Start()
        {
            UpdateHighScore();
            ActivateLevel();
        }

        private void Update()
        {
            if (_bonusTime > 0f)
            {
                _bonusTime -= Time.deltaTime;
            }
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
            _bonusTime = 3f;
        }
        private void ActivateLevel()
        {
            foreach (var level in Levels)
            {
                level.SetActive(false);
            }
            Levels[_level].SetActive(true);
            Waypoints = Levels[_level].transform.Find("Waypoints").GetComponentsInChildren<Transform>()
                .Where(t => t.name != "Waypoints").ToArray();
            _numDots = Levels[_level].transform.Find("Dots").childCount;
        }

        public void DotEaten()
        {
            _score++;
            ScoreText.text = _score.ToString().PadLeft(3, '0');
            if (_score > _highScore)
            {
                PlayerPrefs.SetInt("Score", _score);
                UpdateHighScore();
            }

            _numDots--;
            if (_numDots == 0)
            {
                if (++_level < Levels.Length)
                {
                    NextLevel();
                }
                else
                {
                    // TODO
                }
            }
        }

        private void UpdateHighScore()
        {
            _highScore = PlayerPrefs.GetInt("Score");
            HighScoreText.text = "H:" + _highScore.ToString().PadLeft(3, '0');
        }

        private void NextLevel()
        {
            _bonusTime = 0f;
            ActivateLevel();
            GhostSpeed += 0.1f * (PacManSpeed - GhostSpeed);
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
