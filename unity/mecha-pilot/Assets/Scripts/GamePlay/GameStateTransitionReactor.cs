using System;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    [ExecuteInEditMode]
    public class GameStateTransitionReactor : MonoBehaviour
    {
        public GameManager gameManager;
        public GameState targetState = GameState.Menu;
        public UnityEvent onEnter;
        public UnityEvent onExit;
        private void Awake()
        {
            gameManager ??= GetComponent<GameManager>();
            gameManager ??= FindObjectOfType<GameManager>(true);
            if (gameManager == null)
            {
                enabled = false;
                throw new NullReferenceException("Game manager required");
            }
        }
        private void OnEnable()
        {
            gameManager.GameManagerStateEntered += HandleGameManagerStateEntered;
            gameManager.GameManagerStateExited += HandleGameManagerStateExited;
        }
        private void OnDisable()
        {
            gameManager.GameManagerStateEntered -= HandleGameManagerStateEntered;
            gameManager.GameManagerStateExited -= HandleGameManagerStateExited;
        }
        private void HandleGameManagerStateExited(GameState state)
        {
            if (state == targetState) onExit?.Invoke();
        }
        private void HandleGameManagerStateEntered(GameState state)
        {
            if (state == targetState) onEnter?.Invoke();
        }
        # if UNITY_EDITOR
        private GameState _currentGameState;
        private void Update()
        {
            if (_currentGameState != gameManager.currentState)
            {
                HandleGameManagerStateExited(_currentGameState);
                _currentGameState = gameManager.currentState;
                HandleGameManagerStateEntered(_currentGameState);
            }
        }
        #endif
    }
}
