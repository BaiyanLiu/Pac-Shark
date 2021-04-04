using UnityEngine;

namespace Assets.Scripts.Ghost
{
    public class GhostWaypoints : Ghost
    {
        private int _currWaypoint;
        private int _prevWaypoint;

        protected override void OnLevelChanged()
        {
            _prevWaypoint = _currWaypoint = 0;
        }

        protected override Vector2 NextPos => GameState.Waypoints[GameState.IsBonusTime ? _prevWaypoint : _currWaypoint].position;

        protected override void UpdateNextPos()
        {
            if (transform.position == GameState.Waypoints[GameState.IsBonusTime ? _prevWaypoint : _currWaypoint].position)
            {
                _currWaypoint = (_currWaypoint + GameState.Waypoints.Length + (GameState.IsBonusTime ? -1 : 1)) % GameState.Waypoints.Length;
                _prevWaypoint = (_currWaypoint + GameState.Waypoints.Length - 1) % GameState.Waypoints.Length;
            }
        }

        public override void OnDie()
        {
            _currWaypoint = 0;
        }
    }
}
