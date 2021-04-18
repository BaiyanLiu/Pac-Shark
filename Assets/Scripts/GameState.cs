using System;
using System.Linq;
using Assets.Scripts.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameState : MonoBehaviour
    {
        public static readonly Vector2Int Min = new Vector2Int(-15, -13);
        public static readonly Vector2Int Max = new Vector2Int(15, 13);

        public event EventHandler OnLevelChanged;
        public event EventHandler OnDie;

        public static bool IsPaused { get; set; } = true;

        public Text ScoreText;
        public Text HighScoreText;
        public Text CountdownText;
        public Image[] LivesImages;
        public int Lives;
        public GameObject[] Levels;

        public bool IsBonusTime => _bonusTime > 0f;
        public bool IsDead => Lives == 0;
        
        public const float PacManSpeed = 0.4f;
        public float GhostSpeed { get; private set; }
        public Transform[] Waypoints { get; private set; }

        private int _score;
        private int _highScore;
        private float _bonusTime;
        private int _level;
        private int _numDots;
        private float _countdownTime;

        public static GameState GetGameState(GameObject gameObject)
        {
            return gameObject.scene.GetRootGameObjects().First(o => o.name == "Canvas").GetComponent<GameState>();
        }

        private void Start()
        {
            UpdateHighScore();
            ActivateLevel();
            GhostSpeed = 0.2f * PlayerPrefs.GetInt(Settings.GhostSpeed) / 100f;
        }

        private void Update()
        {
            if (_countdownTime > -1f)
            {
                _countdownTime -= Time.deltaTime;
                var countdownTime = (int) (_countdownTime + 0.99f);
                CountdownText.color = countdownTime switch
                {
                    1 => new Color32(34, 177, 76, 255),
                    2 => new Color32(255, 242, 0, 255),
                    3 => new Color32(255, 0, 0, 255),
                    _ => CountdownText.color
                };
                if (countdownTime > 0)
                {
                    CountdownText.enabled = true;
                    CountdownText.text = countdownTime.ToString();
                }
                else
                {
                    CountdownText.enabled = false;
                }
                IsPaused = CountdownText.enabled;
            }

            if (IsPaused)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
                IsPaused = true;
                return;
            }

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
                LivesImages[i].enabled = false;
            }
            if (IsDead)
            {
                OnDie?.Invoke(this, EventArgs.Empty);
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
            _countdownTime = 3;
        }

        public void DotEaten()
        {
            _score++;
            ScoreText.text = _score.ToString("D4");
            PlayerPrefs.SetInt(Settings.Score, _score);
            if (_score > _highScore)
            {
                PlayerPrefs.SetInt(Settings.HighScore, _score);
                UpdateHighScore();
            }

            _numDots--;
            if (_numDots == 0)
            {
                NextLevel();
            }
        }

        private void UpdateHighScore()
        {
            _highScore = PlayerPrefs.GetInt(Settings.HighScore);
            HighScoreText.text = "H:" + _highScore.ToString("D4");
        }

        private void NextLevel()
        {
            _bonusTime = 0f;
            _level = (_level + 1) % Levels.Length;
            ActivateLevel();
            GhostSpeed += 0.1f * (PacManSpeed - GhostSpeed);
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
