using UnityEngine;

namespace Assets.Scripts.Ghost
{
    public class GhostWaypoints : Ghost
    {
        public Transform[] Waypoints;

        private int _currWaypoint;
        private int _prevWaypoint;

        protected override Vector3 NextPos => Waypoints[GameState.IsBonusTime ? _prevWaypoint : _currWaypoint].position;

        protected override void UpdateNextPos()
        {
            if (transform.position == Waypoints[GameState.IsBonusTime ? _prevWaypoint : _currWaypoint].position)
            {
                _currWaypoint = (_currWaypoint + Waypoints.Length + (GameState.IsBonusTime ? -1 : 1)) % Waypoints.Length;
                _prevWaypoint = (_currWaypoint + Waypoints.Length - 1) % Waypoints.Length;
            }
        }

        public override void OnDie()
        {
            _currWaypoint = 0;
        }
    }
}
