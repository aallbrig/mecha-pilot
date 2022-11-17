using UnityEngine;

namespace Backgrounds
{
    public class BackgroundContainer : MonoBehaviour
    {
        public BoxCollider boundingCollider;
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
        }
        private void OnBecameInvisible() => Debug.Log($"{name} became invisible");
        private void OnBecameVisible() => Debug.Log($"{name} became visible");
        private void OnWillRenderObject() => Debug.Log($"{name} will render object");
        public bool ContainerInDirection(Vector3 direction)
        {
            var containerTransform = _transform ? _transform : transform;
            var theReturn = false;
            var transformDirection = containerTransform.TransformDirection(direction);
            var rayLength = boundingCollider.size.magnitude * 2;
            if (Physics.Raycast(containerTransform.position, transformDirection, out var hit,
                    rayLength, LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer))))
                if (hit.transform != containerTransform &&
                    hit.transform.TryGetComponent<BackgroundContainer>(out var container))
                    theReturn = true;
            return theReturn;
        }
    }
}
