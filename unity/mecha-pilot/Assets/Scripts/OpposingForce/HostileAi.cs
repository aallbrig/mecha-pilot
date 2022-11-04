using Character;
using Gameplay;
using UnityEngine;

namespace OpposingForce
{
    public class HostileAi : MonoBehaviour
    {
        public float detectionDistance = 124f;
        public float closeEnoughDistance;
        public float minSpeed = 6.0f;
        public float maxSpeed = 12.0f;
        private float _distanceToPlayer;
        private GameManager _gameManager;
        private GameObject _player;
        private float _speed;
        private Transform _transform;
        private void Start()
        {
            _transform = transform;
            var playerController = FindObjectOfType<PlayerController>(true);
            if (playerController)
                _player = playerController.gameObject;
        }
        private void LateUpdate()
        {
            if (_player == null) return;

            _distanceToPlayer = Vector3.Distance(_transform.position, _player.transform.position);
            if (_distanceToPlayer < detectionDistance && _distanceToPlayer > closeEnoughDistance)
            {
                var calculatedPosition =
                    Vector3.MoveTowards(_transform.position, _player.transform.position, _speed * Time.deltaTime);
                _transform.position = new Vector3(calculatedPosition.x, calculatedPosition.y, 0);
            }
            _transform.LookAt(_player.transform);
        }
        private void OnEnable() => _speed = Random.Range(minSpeed, maxSpeed);
    }
}
