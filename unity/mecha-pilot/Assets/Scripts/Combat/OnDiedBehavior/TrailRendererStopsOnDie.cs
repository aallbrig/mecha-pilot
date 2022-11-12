using System.Collections.Generic;
using UnityEngine;

namespace Combat.OnDiedBehavior
{
    public class TrailRendererStopsOnDie : MonoBehaviour
    {
        public List<TrailRenderer> trailRenderers = new();
        private ICanDie _ableToDie;
        private float enabledTime;
        private bool timeWaited;
        private void Awake() => trailRenderers.ForEach(DisableTrailRenderer);
        private void Update()
        {
            if (timeWaited) return;

            if (Time.time - enabledTime > 0.5f)
            {
                timeWaited = true;
                trailRenderers.ForEach(EnableTrailRenderer);
            }
        }
        private void OnEnable()
        {
            if (trailRenderers.Count == 0) enabled = false;
            if (_ableToDie == null && TryGetComponent<ICanDie>(out var ableToDie))
                _ableToDie = ableToDie;
            if (_ableToDie == null)
            {
                enabled = false;
            }
            else
            {
                enabledTime = Time.time;
                timeWaited = false;
                _ableToDie.Died += OnDied;
            }
        }
        private void OnDisable()
        {
            if (_ableToDie != null) _ableToDie.Died -= OnDied;
            trailRenderers.ForEach(DisableTrailRenderer);
        }
        private void OnDied(GameObject deadGameObject) => trailRenderers.ForEach(DisableTrailRenderer);
        private void DisableTrailRenderer(TrailRenderer trailerRenderer) => trailerRenderer.emitting = false;
        private void EnableTrailRenderer(TrailRenderer trailRenderer) => trailRenderer.emitting = true;
    }
}
