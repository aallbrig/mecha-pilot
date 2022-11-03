using UnityEngine;

namespace Combat.OnDiedBehavior
{
    public class DeactivateOnDie : MonoBehaviour
    {
        public int framesTilDeactivate = 3;
        private ICanDie _ableToDie;
        private bool _dead;
        private int _framesSinceDied;
        private void Start()
        {
            _ableToDie ??= GetComponent<ICanDie>();

            if (_ableToDie != null) _ableToDie.Died += OnDied;
            else enabled = false;
        }

        private void Update()
        {
            if (!_dead) return;

            _framesSinceDied++;
            if (_framesSinceDied >= framesTilDeactivate)
                gameObject.SetActive(false);
        }
        private void OnEnable() => _dead = false;
        private void OnDied(GameObject deadGameObject)
        {
            _framesSinceDied = 0;
            _dead = true;
        }
    }
}
