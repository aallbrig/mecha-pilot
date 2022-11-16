using UnityEngine;

namespace Backgrounds
{
    public class BackgroundContainer : MonoBehaviour
    {
        public BoxCollider boundingCollider;
        private Bounds _colliderBounds;
        private Transform _transform;
        private void OnEnable()
        {
            boundingCollider ??= GetComponent<BoxCollider>();
            if (boundingCollider == null)
            {
                enabled = false;
                return;
            }
            _transform = transform;
            _colliderBounds = boundingCollider.bounds;
        }
        private void OnBecameInvisible() => Debug.Log($"{name} became invisible");
        private void OnBecameVisible() => Debug.Log($"{name} became visible");
        private void OnWillRenderObject() => Debug.Log($"{name} will render object");
        public bool ContainerInDirection(Vector3 direction)
        {
            var containerTransform = _transform ? _transform : transform;
            var theReturn = false;
            var transformDirection = containerTransform.TransformDirection(direction);
            boundingCollider.enabled = false;
            var rayLength = boundingCollider.size.magnitude;
            if (Physics.Raycast(containerTransform.position, transformDirection, out var hit,
                    rayLength))
                if (hit.transform != containerTransform &&
                    hit.transform.TryGetComponent<BackgroundContainer>(out var container))
                    theReturn = true;
            boundingCollider.enabled = true;
            return theReturn;
        }
    }
}
