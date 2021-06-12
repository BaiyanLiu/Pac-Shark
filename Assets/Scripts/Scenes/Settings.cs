using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes
{
    public class Settings : MonoBehaviour
    {
        public const string BonusDots= "Bonus Dots";
        public const string GhostSpeed = "Ghost Speed";
        public const string HighScore = "High Score";
        public const string Score = "Score";

        public Slider BonusDotsSlider;
        public Slider GhostSpeedSlider;
        public Text BonusDotsValue;
        public Text GhostSpeedValue;
        public Text HighScoreText;

        private int _bonusDots;
        private int _ghostSpeed;

        public static void Init()
        {
            if (!PlayerPrefs.HasKey(BonusDots))
            {
                PlayerPrefs.SetInt(BonusDots, 100);
            }
            if (!PlayerPrefs.HasKey(GhostSpeed))
            {
                PlayerPrefs.SetInt(GhostSpeed, 100);
            }
        }

        [UsedImplicitly]
        private void Start()
        {
            Init();
            BonusDotsSlider.value = _bonusDots = PlayerPrefs.GetInt(BonusDots);
            GhostSpeedSlider.value  = _ghostSpeed = PlayerPrefs.GetInt(GhostSpeed);
            HighScoreText.text = PlayerPrefs.GetInt(HighScore).ToString("D4");
        }

        [UsedImplicitly]
        public void OnBonusDotsChanged(float value)
        {
            _bonusDots = Convert.ToInt16(value);
            BonusDotsValue.text = _bonusDots + "%";
        }

        [UsedImplicitly]
        public void OnGhostSpeedChanged(float value)
        {
            _ghostSpeed = Convert.ToInt16(value);
            GhostSpeedValue.text = _ghostSpeed + "%";
        }

        [UsedImplicitly]
        public void ResetHighScore()
        {
            PlayerPrefs.SetInt(HighScore, 0);
            HighScoreText.text = "0000";
        }

        [UsedImplicitly]
        public void Apply()
        {
            PlayerPrefs.SetInt(BonusDots, _bonusDots);
            PlayerPrefs.SetInt(GhostSpeed, _ghostSpeed);
            SceneManager.LoadScene("Main Menu");
        }

        [UsedImplicitly]
        public void Cancel()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
