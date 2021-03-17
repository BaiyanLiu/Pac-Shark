using UnityEngine;

namespace Assets.Scripts
{
    public class Dot : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name == "PacMan")
            {
                Destroy(gameObject);
            }
        }
    }
}
