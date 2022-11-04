using Character;
using Gameplay;
using UnityEngine;

namespace OpposingForce
{
    [RequireComponent(typeof(Rigidbody))]
    public class HostileAi : MonoBehaviour
    {
        public float detectionDistance = 124f;
        public float closeEnoughDistance;
        public float minSpeed = 6.0f;
        public float maxSpeed = 12.0f;
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
            var playerController = FindObjectOfType<PlayerController>(true);
            if (playerController)
                _player = playerController.gameObject;
        }
        private void LateUpdate()
        {
            if (_player == null) return;

            _transform.LookAt(_player.transform);
            _distanceToPlayer = Vector3.Distance(_transform.position, _player.transform.position);
            if (_distanceToPlayer < detectionDistance && _distanceToPlayer > closeEnoughDistance)
            {
                var calculatedPosition =
                    Vector3.MoveTowards(_transform.position, _player.transform.position, _speed * Time.deltaTime);
                _transform.position = new Vector3(calculatedPosition.x, calculatedPosition.y, 0);
            }
        }
        private void OnEnable()
        {
            _rigidBody.velocity = Vector3.zero;
            _speed = Random.Range(minSpeed, maxSpeed);
        }
    }
}
