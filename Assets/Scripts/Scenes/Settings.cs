using System;
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

        private void Start()
        {
            if (!PlayerPrefs.HasKey(BonusDots))
            {
                PlayerPrefs.SetInt(BonusDots, 100);
            }
            if (!PlayerPrefs.HasKey(GhostSpeed))
            {
                PlayerPrefs.SetInt(GhostSpeed, 100);
            }
            BonusDotsSlider.value = _bonusDots = PlayerPrefs.GetInt(BonusDots);
            GhostSpeedSlider.value  = _ghostSpeed = PlayerPrefs.GetInt(GhostSpeed);
            HighScoreText.text = PlayerPrefs.GetInt(HighScore).ToString("D3");
        }

        public void OnBonusDotsChanged(float value)
        {
            _bonusDots = Convert.ToInt16(value);
            BonusDotsValue.text = _bonusDots + "%";
        }

        public void OnGhostSpeedChanged(float value)
        {
            _ghostSpeed = Convert.ToInt16(value);
            GhostSpeedValue.text = _ghostSpeed + "%";
        }

        public void ResetHighScore()
        {
            PlayerPrefs.SetInt(HighScore, 0);
            HighScoreText.text = "000";
        }

        public void Apply()
        {
            PlayerPrefs.SetInt(BonusDots, _bonusDots);
            PlayerPrefs.SetInt(GhostSpeed, _ghostSpeed);
            SceneManager.LoadScene("Main Menu");
        }

        public void Cancel()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
