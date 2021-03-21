using UnityEngine.UI;

namespace Assets.Scripts
{
    public static class GameState
    {
        private static int _score;

        public static void UpdateScore(Text text, int delta)
        {
            _score += delta;
            text.text = _score.ToString();
        }
    }
}
