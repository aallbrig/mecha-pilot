using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;
using Gameplay;
using Player;
using UnityEngine;

namespace OpposingForce
{
    [RequireComponent(typeof(Rigidbody))]
    public class HostileAi : MonoBehaviour, IProvideBehaviorTree
    {
        public float detectionDistance = 124f;
        public float closeEnoughDistance;
        public float minSpeed = 6.0f;
        public float maxSpeed = 12.0f;
        [SerializeField] private BehaviorTree tree;
        private float _distanceToPlayer;
        private GameManager _gameManager;
        private GameObject _player;
        private Rigidbody _rigidBody;
        private float _speed;
        private Transform _transform;
        private void Awake()
        {
            _transform = transform;
            _rigidBody = GetComponent<Rigidbody>();
            tree = GetBehaviorTree();
            var playerController = FindObjectOfType<PlayerController>(true);
            if (playerController)
                _player = playerController.gameObject;
        }
        private void Update() => tree.Tick();
        private void OnEnable()
        {
            _rigidBody.velocity = Vector3.zero;
            _speed = Random.Range(minSpeed, maxSpeed);
        }
        public BehaviorTree GetBehaviorTree()
        {
            var bt = new BehaviorTreeBuilder(gameObject)
                .Parallel()
                .Do("Current distance", () =>
                {
                    if (_player == null) return TaskStatus.Failure;

                    _distanceToPlayer = Vector3.Distance(_transform.position, _player.transform.position);
                    return TaskStatus.Success;
                })
                .Sequence("Move to player")
                .Condition("Can I detect the player?", () => _distanceToPlayer < detectionDistance)
                .Condition("Should I get closer?", () => _distanceToPlayer > closeEnoughDistance)
                .Do("Move closer to player", () =>
                {
                    var calculatedPosition =
                        Vector3.MoveTowards(_transform.position, _player.transform.position, _speed * Time.deltaTime);
                    _transform.position = new Vector3(calculatedPosition.x, calculatedPosition.y, 0);
                    return TaskStatus.Success;
                })
                .End()
                .Do("Look at player", () =>
                {
                    _transform.LookAt(_player.transform);
                    return TaskStatus.Success;
                })
                .End()
                .Build();
            return bt;
        }
    }

    public interface IProvideBehaviorTree
    {
        public BehaviorTree GetBehaviorTree();
    }
}
