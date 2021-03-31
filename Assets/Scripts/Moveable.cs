using System.Linq;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Moveable : MonoBehaviour
    {
        protected bool IsValid(Vector2 dir, params int[] layers)
        {
            var layerMask = (1 << 31) + layers.Sum(layer => 1 << layer);
            var hit = Physics2D.CircleCast(transform.position, GetComponent<CircleCollider2D>().radius, dir, 1f, layerMask);
            return hit.collider == null;
        }
    }
}
