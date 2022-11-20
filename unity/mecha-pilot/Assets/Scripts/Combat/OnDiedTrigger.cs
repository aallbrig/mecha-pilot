using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class OnDiedTrigger : MonoBehaviour
    {
        public GameObject gameObjectAbleToDie;
        public UnityEvent onDied;
        private ICanDie _ableToDie;
        private void Awake() => Setup();

        private void OnEnable() => _ableToDie.Died += HandleOnDied;
        private void OnDisable() => _ableToDie.Died -= HandleOnDied;
        private void OnValidate() => Setup();
        public void HandleOnDied(GameObject deadGameObject) => onDied?.Invoke();
        private void Setup()
        {
            if (!RegisterGameObjectAbleToDie())
            {
                Debug.LogError($"{name} unable to find something able to die");
                enabled = false;
            }
        }
        private bool RegisterGameObjectAbleToDie()
        {
            gameObjectAbleToDie ??= gameObject;
            if (gameObjectAbleToDie.TryGetComponent<ICanDie>(out var ableToDie))
            {
                _ableToDie = ableToDie;
                return true;
            }
            return false;
        }
    }
}
