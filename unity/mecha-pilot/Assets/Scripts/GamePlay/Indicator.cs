using UnityEngine;
using UnityEngine.Animations;

namespace Gameplay
{
    [RequireComponent(typeof(LookAtConstraint), typeof(PositionConstraint))]
    public class Indicator : MonoBehaviour
    {
        public Transform follow;
        public Transform track;
        private LookAtConstraint _lookAtConstraint;
        private PositionConstraint _positionConstraint;
        private void Awake()
        {
            _lookAtConstraint = GetComponent<LookAtConstraint>();
            _positionConstraint = GetComponent<PositionConstraint>();
        }
        private void OnEnable()
        {
            SyncLookAtConstraint();
            SyncPositionConstraint();
        }
        private void SyncLookAtConstraint()
        {
            if (!track) return;
            if (_lookAtConstraint.sourceCount > 0)
                for (var i = _lookAtConstraint.sourceCount - 1; i >= 0; i--)
                    if (_lookAtConstraint.GetSource(i).sourceTransform != track)
                        _lookAtConstraint.RemoveSource(i);
            if (_lookAtConstraint.sourceCount == 0)
                _lookAtConstraint.AddSource(new ConstraintSource { sourceTransform = track, weight = 1.0f });
        }
        private void SyncPositionConstraint()
        {
            if (!follow) return;
            if (_positionConstraint.sourceCount > 0)
                for (var i = _positionConstraint.sourceCount - 1; i >= 0; i--)
                    if (_positionConstraint.GetSource(i).sourceTransform != follow)
                        _positionConstraint.RemoveSource(i);
            if (_positionConstraint.sourceCount == 0)
                _positionConstraint.AddSource(new ConstraintSource { sourceTransform = follow, weight = 1.0f });
        }
        public void SetTrack(Transform trackTarget)
        {
            track = trackTarget;
            SyncLookAtConstraint();
            SyncPositionConstraint();
        }
    }
}
