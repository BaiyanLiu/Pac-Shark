using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Assets.Scripts.Scenes
{
    public class Intro : MonoBehaviour
    {
        public VideoPlayer IntroVideo;

        [UsedImplicitly]
        private void Start()
        {
            PlayerPrefs.SetInt(Settings.SkipIntro, 1);
            IntroVideo.loopPointReached += source => StartGame();
        }

        [UsedImplicitly]
        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetTouch(0).phase == TouchPhase.Began)
            {
                StartGame();
            }
        }

        private static void StartGame()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
