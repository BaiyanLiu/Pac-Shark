using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts
{
    public abstract class Moveable : MonoBehaviour
    {
        protected Rigidbody2D Rigidbody;
        protected CircleCollider2D Collider;

        [UsedImplicitly]
        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<CircleCollider2D>();
            OnStart();
        }

        protected virtual void OnStart() {}

        protected bool IsValid(Vector2 dir, params int[] layers)
        {
            return IsValid(transform.position, dir, layers);
        }

        protected bool IsValid(Vector2 pos, Vector2 dir, params int[] layers)
        {
            var layerMask = (1 << 31) + layers.Sum(layer => 1 << layer);
            var hit = Physics2D.CircleCast(pos, Collider.radius, dir, 1f, layerMask);
            return hit.collider == null;
        }
    }
}
