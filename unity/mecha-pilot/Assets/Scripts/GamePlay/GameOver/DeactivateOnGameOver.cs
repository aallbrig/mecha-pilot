using UnityEngine;

namespace Gameplay.GameOver
{
    public class DeactivateOnGameOver : MonoBehaviour
    {
        private GameManager _gameManager;
        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>(true);
            if (_gameManager == null) Deactivate();
        }
        private void OnEnable() => _gameManager.GameIsOver += Deactivate;
        private void OnDisable() => _gameManager.GameIsOver -= Deactivate;
        private void Deactivate() => gameObject.SetActive(false);
    }
}
