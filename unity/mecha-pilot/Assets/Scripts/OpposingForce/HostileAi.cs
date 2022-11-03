using Character;
using UnityEngine;

namespace OpposingForce
{
    public class HostileAi : MonoBehaviour
    {
        public float detectionDistance = 20f;
        public float closeEnoughDistance = 5f;
        public float speed = 3.0f;
        private GameObject _player;
        private Transform _transform;
        private void Start()
        {
            _transform = transform;
            var playerController = FindObjectOfType<PlayerController>();
            if (playerController) _player = FindObjectOfType<PlayerController>().gameObject;
        }
        private void Update()
        {
            if (_player == null) return;

            var distanceToPlayer = Vector3.Distance(_transform.position, _player.transform.position);
            if (distanceToPlayer < detectionDistance && distanceToPlayer > closeEnoughDistance)
            {
                var vectorToPlayer = (_player.transform.position - _transform.position).normalized;
                _transform.Translate(vectorToPlayer * speed * Time.deltaTime);
                _transform.position = new Vector3(_transform.position.x, _transform.position.y, 0);
            }
        }
        private void LateUpdate()
        {
            if (_player == null) return;
            _transform.LookAt(_player.transform);
        }
    }
}
