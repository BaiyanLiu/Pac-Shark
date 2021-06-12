using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes
{
    public class MainMenu : MonoBehaviour
    {
        [UsedImplicitly]
        private void Start()
        {
            if (!PlayerPrefs.HasKey(Scenes.Settings.BonusDots))
            {
                PlayerPrefs.SetInt(Scenes.Settings.BonusDots, 100);
            }
            if (!PlayerPrefs.HasKey(Scenes.Settings.GhostSpeed))
            {
                PlayerPrefs.SetInt(Scenes.Settings.GhostSpeed, 100);
            }
            if (!PlayerPrefs.HasKey(Scenes.Settings.SkipIntro))
            {
                PlayerPrefs.SetInt(Scenes.Settings.SkipIntro, 0);
            }
        }

        [UsedImplicitly]
        public void StartGame()
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt(Scenes.Settings.SkipIntro) == 0 ? "Intro" : "Game");
        }

        [UsedImplicitly]
        public void Settings()
        {
            SceneManager.LoadScene("Settings");
        }

        [UsedImplicitly]
        public void Exit()
        {
            Application.Quit();
        }
    }
}
