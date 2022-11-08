using System;
using CleverCrow.Fluid.FSMs;
using Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public enum GameState
    {
        Menu,
        GamePlay,
        GameOver
    }

    public class GameManager : MonoBehaviour, IInitializePlay
    {
        public GameObject player;
        public float enoughTimeHasPassed = 2f;
        public Button playButton;
        public Button resetButton;
        public GameState startingGameState = GameState.Menu;
        private GameState _currentState;
        private IFsm _fsm;
        private bool _gameOver;
        private float _gameOverTime;
        private bool _playerWantsToPlayGame;
        private bool _resetButtonClicked;
        private void Start()
        {
            if (playButton) playButton.onClick.AddListener(PlayGame);
            if (resetButton) resetButton.onClick.AddListener(() => _resetButtonClicked = true);
            if (player && player.TryGetComponent<ICanDie>(out var ableToDie))
                ableToDie.Died += _ =>
                {
                    _gameOver = true;
                    _gameOverTime = Time.time;
                };

            _fsm = GetFiniteStateMachineDefinition().Build();
        }
        private void Update() => _fsm.Tick();
        private void OnValidate() => _fsm = GetFiniteStateMachineDefinition().Build();

        public void PlayGame() => _playerWantsToPlayGame = true;
        private FsmBuilder GetFiniteStateMachineDefinition() => new FsmBuilder()
            .Owner(gameObject)
            .Default(startingGameState)
            .State(GameState.Menu, builder =>
            {
                builder
                    .Enter(_ =>
                    {
                        _playerWantsToPlayGame = false;
                        GameManagerStateEntered?.Invoke(GameState.Menu);
                    })
                    .SetTransition(nameof(GameState.GamePlay), GameState.GamePlay)
                    .Exit(_ =>
                    {
                        GameManagerStateExited?.Invoke(GameState.Menu);
                    })
                    .Update(action =>
                    {
                        if (_playerWantsToPlayGame) action.Transition(nameof(GameState.GamePlay));
                    });

            })
            .State(GameState.GameOver, builder =>
            {
                builder
                    .Enter(_ =>
                    {
                        _resetButtonClicked = false;
                        GameManagerStateEntered?.Invoke(GameState.GameOver);
                    })
                    .Exit(_ =>
                    {
                        GameManagerStateExited?.Invoke(GameState.GameOver);
                        GameIsOver?.Invoke();
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
                        GameManagerStateEntered?.Invoke(GameState.GamePlay);
                    })
                    .Exit(_ =>
                    {
                        GameManagerStateExited?.Invoke(GameState.GamePlay);
                    })
                    .SetTransition(nameof(GameState.GameOver), GameState.GameOver)
                    .Update(action =>
                    {
                        if (_gameOver && Time.time - _gameOverTime > enoughTimeHasPassed)
                            action.Transition(nameof(GameState.GameOver));
                    });
            });

        public event Action<GameState> GameManagerStateEntered;

        public event Action<GameState> GameManagerStateExited;

        public event Action GameIsOver;
    }

    public interface IInitializePlay
    {
        public void PlayGame();
    }
}
