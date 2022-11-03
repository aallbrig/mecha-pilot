using UnityEngine;

namespace Combat.OnDiedBehavior
{
    public class DeactivateOnDie : MonoBehaviour
    {
        private ICanDie _ableToDie;
        private void Start()
        {
            _ableToDie ??= GetComponent<ICanDie>();

            if (_ableToDie != null) _ableToDie.Died += OnDied;
            else enabled = false;
        }
        private void OnDied(GameObject deadGameObject) => deadGameObject.SetActive(false);
    }
}
