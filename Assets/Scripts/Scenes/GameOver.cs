using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Scenes
{
    public class GameOver : MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
