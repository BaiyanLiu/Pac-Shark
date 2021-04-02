using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Ghost
{
    public class GhostRandom : Ghost
    {
        private Vector2 _dest;
        private Vector2 _dir;

        protected override void OnStart()
        {
            Reset();
        }

        private void Reset()
        {
            _dest = OriginalPosition;
        }

        protected override Vector2 NextPos => _dest;

        protected override void UpdateNextPos()
        {
            if ((Vector2) transform.position == _dest)
            {
                var dir = Vector2.zero;
                do
                {
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            dir = Vector2.up;
                            break;
                        case 1:
                            dir = Vector2.right;
                            break;
                        case 2:
                            dir = Vector2.down;
                            break;
                        case 3:
                            dir = Vector2.left;
                            break;
                    }
                    _dest = (Vector2) transform.position + dir;
                } while (dir == -_dir || _dest.x < GameState.Min.x || _dest.x > GameState.Max.x || !IsValid(dir));
                _dir = dir;
            }
        }

        public override void OnDie()
        {
            Reset();
        }
    }
}
