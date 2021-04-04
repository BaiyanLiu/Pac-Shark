using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.PacMan;
using UnityEngine;

namespace Assets.Scripts.Ghost
{
    public class GhostFollow : Ghost
    {
        public GameObject PacMan;

        private Vector2 _dest;

        protected override void OnStart()
        {
            Reset();
        }

        protected override void OnLevelChanged()
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
            if (GameState.IsDead)
            {
                return;
            }

            if ((Vector2) transform.position == _dest)
            {
                var dest = GameState.IsBonusTime ? OriginalPosition : PacMan.GetComponent<Move>().Dest;
                dest.x = Math.Max(dest.x, GameState.Min.x);
                dest.x = Math.Min(dest.x, GameState.Max.x);
                _dest = ShortestPath(dest);
            }
        }

        protected Vector2 ShortestPath(Vector2 dest)
        {
            if ((Vector2) transform.position == dest)
            {
                return transform.position;
            }

            var dist = new Dictionary<Vector2, int>();
            var score = new Dictionary<Vector2, float>();
            for (var x = GameState.Min.x; x <= GameState.Max.x; x++)
            {
                for (var y = GameState.Min.y; y <= GameState.Max.y; y++)
                {
                    var key = new Vector2Int(x, y);
                    dist[key] = int.MaxValue;
                    score[key] = float.MaxValue;
                }
            }
            var pos = Vector2Int.RoundToInt(transform.position);
            dist[pos] = 0;
            score[pos] = Vector2.Distance(transform.position, dest);
            var openNodes = new Dictionary<Vector2, float>(score);
            var cameFrom = new Dictionary<Vector2, Vector2>();

            while (openNodes.Count > 0)
            {
                var curr = openNodes.OrderBy(x => x.Value).First().Key;
                if (curr == dest)
                {
                    while (cameFrom.ContainsKey(curr))
                    {
                        if ((Vector2) transform.position == cameFrom[curr])
                        {
                            return curr;
                        }
                        curr = cameFrom[curr];
                    }
                }
                openNodes.Remove(curr);

                var neighbors = (
                    from dir in new[] {Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down} 
                    let key = Vector2Int.RoundToInt(curr + dir)
                    where IsValid(curr, dir) && dist.ContainsKey(key)
                    select key).ToList();

                foreach (var neighbor in neighbors)
                {
                    var altDist = dist[curr] + Dist(curr, neighbor);
                    if (altDist < dist[neighbor])
                    {
                        dist[neighbor] = altDist;
                        openNodes[neighbor] = score[neighbor] = altDist + Vector2.Distance(neighbor, dest);
                        cameFrom[neighbor] = curr;
                    }
                }
            }

            return Vector2.zero;
        }

        private int Dist(Vector2 from, Vector2 to)
        {
            return Math.Abs((int) (from.x - to.x)) + Math.Abs((int) (from.y - to.y));
        }

        public override void OnDie()
        {
            Reset();
        }
    }
}
