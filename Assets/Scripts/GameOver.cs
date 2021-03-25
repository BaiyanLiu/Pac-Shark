using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class GameOver : MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene("Game");
        }
    }
}
