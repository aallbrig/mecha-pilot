using System.Collections.Generic;
using UnityEngine;

namespace Combat.OnDiedBehavior
{
    public class TrailRendererStopsOnDie : MonoBehaviour
    {
        public List<TrailRenderer> trailRenderers = new();
        private void Start()
        {
            if (TryGetComponent<ICanDie>(out var ableToDie))
                ableToDie.Died += _ => trailRenderers.ForEach(DisableTrailRenderer);
        }
        private void OnEnable()
        {
            if (trailRenderers.Count == 0) enabled = false;
            trailRenderers.ForEach(EnableTrailRenderer);
        }
        private void OnDisable() => trailRenderers.ForEach(DisableTrailRenderer);
        private void DisableTrailRenderer(TrailRenderer trailerRenderer) => trailerRenderer.emitting = false;
        private void EnableTrailRenderer(TrailRenderer trailRenderer) => trailRenderer.emitting = true;
    }
}
