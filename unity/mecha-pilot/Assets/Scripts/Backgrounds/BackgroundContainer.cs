using System;
using System.Collections.Generic;
using UnityEngine;

namespace Backgrounds
{
    [Serializable]
    public struct MissingBackgroundContainerReport
    {
        public Vector3Int ExpectedCoordinates { get; set; }

        public Vector3 ExpectedLocation { get; set; }

        public Vector3Int RelativeDirection { get; set; }
    }

    public class BackgroundContainer : MonoBehaviour
    {

        private static readonly Vector3Int[] Directions =
        {
            Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left
        };

        public BoxCollider boundingCollider;
        public Vector3Int coordinates;

        private readonly Dictionary<Vector3Int, BackgroundContainer> _nearbyBackgroundContainer = new();

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
        public MissingBackgroundContainerReport[] AuditMissingContainers()
        {
            var containerTransform = _transform ? _transform : transform;
            var missingContainers = new List<MissingBackgroundContainerReport>();
            foreach (var direction in Directions)
                if (!ContainerInDirection(direction))
                {
                    var colliderSize = boundingCollider.size;
                    missingContainers.Add(new MissingBackgroundContainerReport
                    {
                        ExpectedCoordinates = coordinates + direction,
                        ExpectedLocation = containerTransform.position + new Vector3(
                            colliderSize.x * direction.x,
                            colliderSize.y * direction.y,
                            colliderSize.z * direction.z
                        ),
                        RelativeDirection = direction
                    });
                }
            return missingContainers.ToArray();
        }
        public bool RegisterContainer(Vector3Int direction, BackgroundContainer otherBackgroundContainer) =>
            _nearbyBackgroundContainer.TryAdd(direction, otherBackgroundContainer);
        public bool ContainerInDirection(Vector3Int direction)
        {
            if (_nearbyBackgroundContainer.TryGetValue(direction, out _)) return true;

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
