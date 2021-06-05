using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Scenes
{
    public class GameOver : MonoBehaviour
    {
        public Text ScoreTest;
        public Text HighScoreText;

        [UsedImplicitly]
        private void Start()
        {
            ScoreTest.text = PlayerPrefs.GetInt(Settings.Score).ToString("D4");
            HighScoreText.text = PlayerPrefs.GetInt(Settings.HighScore).ToString("D4");
        }

        [UsedImplicitly]
        public void Restart()
        {
            SceneManager.LoadScene("Game");
        }

        [UsedImplicitly]
        public void MainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
