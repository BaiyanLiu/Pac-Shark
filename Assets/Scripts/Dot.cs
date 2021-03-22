using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Dot : MonoBehaviour
    {
        public Text ScoreText;

        public Sprite BigSprite;
        private bool _isBig;

        void Start()
        {
            _isBig = Random.value < 0.1f;
            if (_isBig)
            {
                GetComponent<SpriteRenderer>().sprite = BigSprite;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "PacMan")
            {
                Destroy(gameObject);
                GameState.UpdateScore(ScoreText, _isBig ? 5 : 1);
            }
        }
    }
}
