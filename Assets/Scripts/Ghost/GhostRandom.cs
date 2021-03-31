using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Ghost
{
    public class GhostRandom : Ghost
    {
        private Vector3 _dest;
        private Vector2 _dir;

        protected override void OnStart()
        {
            _dir = Vector2.up;
        }

        protected override Vector3 NextPos => _dest;

        protected override void UpdateNextPos()
        {
            if (transform.position == _dest)
            {
                while (!IsValid(_dir))
                {
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            _dir = Vector2.up;
                            break;
                        case 1:
                            _dir = Vector2.right;
                            break;
                        case 2:
                            _dir = Vector2.down;
                            break;
                        case 3:
                            _dir = Vector2.left;
                            break;
                    }
                }
                _dest = (Vector2) transform.position + _dir;
            }
        }

        public override void OnDie()
        {
            _dest = OriginalPosition;
            _dir = Vector2.up;
        }
    }
}
