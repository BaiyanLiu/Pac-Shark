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
        public Image[] LivesImages;
        public int Lives;
        public GameObject[] Levels;

        public bool IsBonusTime => _bonusTime > 0f;
        public bool IsDead => Lives == 0;
        public Transform[] Waypoints { get; private set; }
        public float GhostSpeed { get; private set; }

        private int _score;
        private float _bonusTime;
        private int _level = 1;

        public static GameState GetGameState(GameObject gameObject)
        {
            return gameObject.scene.GetRootGameObjects().First(o => o.name == "Canvas").GetComponent<GameState>();
        }

        private void Start()
        {
            ActivateLevel();
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
        private void ActivateLevel()
        {
            foreach (var level in Levels)
            {
                level.SetActive(false);
            }
            Levels[_level].SetActive(true);
            Waypoints = Levels[_level].transform.Find("Waypoints")
                .GetComponentsInChildren<Transform>()
                .Where(t => t.name != "Waypoints")
                .OrderBy(t => t.name)
                .ToArray();
            GhostSpeed = 0.2f + 0.02f * _level;
        }

        public void NextLevel()
        {
            _level += 1;
            ActivateLevel();
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
