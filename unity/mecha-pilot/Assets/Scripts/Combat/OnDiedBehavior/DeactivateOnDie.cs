using UnityEngine;

namespace Combat.OnDiedBehavior
{
    public class DeactivateOnDie : MonoBehaviour
    {
        [SerializeReference] private ICanDie _ableToDie;
        private bool _active;
        private void Start()
        {
            _ableToDie ??= GetComponent<ICanDie>();
            _active = _ableToDie != null;
            if (_ableToDie != null) _ableToDie.Died += OnDied;
        }
        private void OnDied(GameObject deadGameObject) => deadGameObject.SetActive(false);
    }
}
