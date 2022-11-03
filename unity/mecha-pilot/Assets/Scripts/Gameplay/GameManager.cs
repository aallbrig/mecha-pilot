using System.Collections.Generic;
using CleverCrow.Fluid.FSMs;
using Combat;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gameplay
{
    public enum GameState
    {
        Menu,
        GamePlay,
        GameOver
    }

    public class GameManager : MonoBehaviour
    {
        public GameObject player;
        public Button playButton;
        public Button resetButton;
        public List<UnityEvent> onMenuEnter;
        public List<UnityEvent> onMenuExit;
        public List<UnityEvent> onGamePlayEnter;
        public List<UnityEvent> onGamePlayExit;
        public List<UnityEvent> onGameOverEnter;
        public List<UnityEvent> onGameOverExit;
        private GameState _currentState;
        private IFsm _fsm;
        private bool _gameOver;
        private bool _playGameButton;
        private bool _resetButtonClicked;
        private void Start()
        {
            if (playButton) playButton.onClick.AddListener(() => _playGameButton = true);
            if (resetButton) resetButton.onClick.AddListener(() => _resetButtonClicked = true);
            if (player && player.TryGetComponent<ICanDie>(out var ableToDie))
                ableToDie.Died += _ => _gameOver = true;

            _fsm = new FsmBuilder()
                .Owner(gameObject)
                .Default(GameState.Menu)
                .State(GameState.Menu, builder =>
                {
                    builder
                        .Enter(_ =>
                        {
                            _playGameButton = false;
                            onMenuEnter.ForEach(fn => fn.Invoke());
                        })
                        .SetTransition(nameof(GameState.GamePlay), GameState.GamePlay)
                        .Exit(_ => onMenuExit.ForEach(fn => fn.Invoke()))
                        .Update(action =>
                        {
                            if (_playGameButton) action.Transition(nameof(GameState.GamePlay));
                        });

                })
                .State(GameState.GameOver, builder =>
                {
                    builder
                        .Enter(_ =>
                        {
                            _resetButtonClicked = false;
                            onGameOverEnter.ForEach(fn => fn.Invoke());
                        })
                        .Exit(_ =>
                        {
                            onGameOverExit.ForEach(fn => fn.Invoke());
                        })
                        .SetTransition(nameof(GameState.Menu), GameState.Menu)
                        .Update(action =>
                        {
                            if (_resetButtonClicked) action.Transition(nameof(GameState.Menu));
                        });
                })
                .State(GameState.GamePlay, builder =>
                {
                    builder
                        .Enter(_ =>
                        {
                            _gameOver = false;
                            onGamePlayEnter.ForEach(fn => fn.Invoke());
                        })
                        .Exit(_ =>
                        {
                            onGamePlayExit.ForEach(fn => fn.Invoke());
                        })
                        .SetTransition(nameof(GameState.GameOver), GameState.GameOver)
                        .Update(action =>
                        {
                            if (_gameOver) action.Transition(nameof(GameState.GameOver));
                        });
                })
                .Build();
        }
        private void Update() => _fsm.Tick();
    }
}
