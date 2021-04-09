using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes
{
    public class GameOver : MonoBehaviour
    {
        public Text ScoreTest;
        public Text HighScoreText;

        private void Start()
        {
            ScoreTest.text = "Score: " + PlayerPrefs.GetInt(Settings.Score).ToString("D3");
            HighScoreText.text = "High Score: " + PlayerPrefs.GetInt(Settings.HighScore).ToString("D3");
        }

        public void Restart()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
