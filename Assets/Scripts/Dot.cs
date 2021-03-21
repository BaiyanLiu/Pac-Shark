using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Dot : MonoBehaviour
    {
        public Text ScoreText;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "PacMan")
            {
                Destroy(gameObject);
                GameState.UpdateScore(ScoreText, 1);
            }
        }
    }
}
