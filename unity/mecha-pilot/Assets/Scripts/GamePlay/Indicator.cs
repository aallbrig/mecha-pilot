using UnityEngine;
using UnityEngine.Animations;

namespace Gameplay
{
    [RequireComponent(typeof(LookAtConstraint))]
    public class Indicator : MonoBehaviour
    {
        public Transform follow;
        public Transform track;
        public float distanceAway = 3f;
        public Vector3 offset = Vector3.zero;
        private LookAtConstraint _lookAtConstraint;
        private Transform _transform;
        private void Update()
        {
            if (track == null || follow == null) return;

            _transform.position = CalculateIndicatorPosition();
        }
        private void OnEnable()
        {
            _lookAtConstraint = GetComponent<LookAtConstraint>();
            _transform = transform;
            SyncLookAtConstraint();
        }
        public Vector3 CalculateIndicatorPosition()
        {
            var followPosition = follow.position;
            var direction = (followPosition - track.position).normalized;
            return followPosition - offset - direction * distanceAway;
        }
        private void SyncLookAtConstraint()
        {
            if (_lookAtConstraint.sourceCount > 0)
                for (var i = _lookAtConstraint.sourceCount - 1; i >= 0; i--)
                    _lookAtConstraint.RemoveSource(i);
            _lookAtConstraint.AddSource(new ConstraintSource { sourceTransform = track, weight = 1.0f });
        }
        public void SetTrack(Transform trackTarget)
        {
            track = trackTarget;
            SyncLookAtConstraint();
        }
    }
}
