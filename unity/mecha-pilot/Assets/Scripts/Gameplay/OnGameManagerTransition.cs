using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
    [ExecuteInEditMode]
    public class OnGameManagerTransition : MonoBehaviour
    {
        public GameManager gameManager;
        public List<UnityEvent> onGameOverEnter;
        public List<UnityEvent> onGameOverExit;
        public List<UnityEvent> onGamePlayEnter;
        public List<UnityEvent> onGamePlayExit;
        public List<UnityEvent> onMenuEnter;
        public List<UnityEvent> onMenuExit;
        private GameState _currentGameState;
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
        # if UNITY_EDITOR
        private void Update()
        {
            if (_currentGameState != gameManager.currentState)
            {
                if (_currentGameState != null) HandleGameManagerStateExited(_currentGameState);
                _currentGameState = gameManager.currentState;
                HandleGameManagerStateEntered(_currentGameState);
            }
        }
        #endif
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
            switch (state)
            {
                case GameState.Menu:
                    onMenuExit.ForEach(fn => fn.Invoke());
                    break;
                case GameState.GamePlay:
                    onGamePlayExit.ForEach(fn => fn.Invoke());
                    break;
                case GameState.GameOver:
                    onGameOverExit.ForEach(fn => fn.Invoke());
                    break;
                default:
                    Debug.Log($"Unhandled game state exit {state}");
                    break;
            }
        }
        private void HandleGameManagerStateEntered(GameState state)
        {
            switch (state)
            {
                case GameState.Menu:
                    onMenuEnter.ForEach(fn => fn.Invoke());
                    break;
                case GameState.GamePlay:
                    onGamePlayEnter.ForEach(fn => fn.Invoke());
                    break;
                case GameState.GameOver:
                    onGameOverEnter.ForEach(fn => fn.Invoke());
                    break;
                default:
                    Debug.Log($"Unhandled game state enter {state}");
                    break;
            }
        }
    }
}
