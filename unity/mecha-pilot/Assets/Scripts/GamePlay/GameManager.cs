using System;
using CleverCrow.Fluid.FSMs;
using Combat;
using UnityEngine;

namespace Gameplay
{
    public enum GameState
    {
        Menu,
        GamePlay,
        GameOver,
        ScoreBoard
    }

    [ExecuteInEditMode]
    public class GameManager : MonoBehaviour, IInitializePlay
    {
        public GameObject player;
        public float secondsBeforeTransitionToGameOver = 2f;
        public GameState startingGameState = GameState.Menu;
        public GameState currentState;
        private IFsm _fsm;
        private bool _gameOver;
        private float _gameOverTime;
        private void Awake() => GameManagerStateEntered += gameState => currentState = gameState;
        private void Start()
        {
            _fsm = GetFiniteStateMachineDefinition().Build();
            if (player && player.TryGetComponent<ICanDie>(out var ableToDie))
                ableToDie.Died += _ =>
                {
                    _gameOver = true;
                    _gameOverTime = Time.time;
                };
        }
        private void Update() => _fsm.Tick();
        private void OnValidate() => _fsm = GetFiniteStateMachineDefinition().Build();
        public void PlayGame() => SetState(GameState.GamePlay);

        public void Menu() => SetState(GameState.Menu);
        public void ScoreBoard() => SetState(GameState.ScoreBoard);
        public void GameOver() => SetState(GameState.GameOver);
        private void SetState(GameState state) => _fsm.SetState(state);
        private FsmBuilder GetFiniteStateMachineDefinition() => new FsmBuilder()
            .Owner(gameObject)
            .Default(startingGameState)
            .State(GameState.Menu, builder =>
            {
                builder
                    .Enter(_ =>
                    {
                        GameManagerStateEntered?.Invoke(GameState.Menu);
                    })
                    .SetTransition(nameof(GameState.GamePlay), GameState.GamePlay)
                    .Exit(_ =>
                    {
                        GameManagerStateExited?.Invoke(GameState.Menu);
                    })
                    .Update(action => {});

            })
            .State(GameState.GameOver, builder =>
            {
                builder
                    .Enter(_ =>
                    {
                        GameManagerStateEntered?.Invoke(GameState.GameOver);
                    })
                    .Exit(_ =>
                    {
                        GameManagerStateExited?.Invoke(GameState.GameOver);
                        GameIsOver?.Invoke();
                    })
                    .SetTransition(nameof(GameState.Menu), GameState.Menu)
                    .Update(action => {});
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
                        if (_gameOver && Time.time - _gameOverTime > secondsBeforeTransitionToGameOver)
                            action.Transition(nameof(GameState.GameOver));
                    });
            })
            .State(GameState.ScoreBoard, builder =>
            {
                builder
                    .Enter(_ =>
                    {
                        GameManagerStateEntered?.Invoke(GameState.ScoreBoard);
                    })
                    .Exit(_ =>
                    {
                        GameManagerStateExited?.Invoke(GameState.ScoreBoard);
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
