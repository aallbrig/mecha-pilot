using System;
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
            _player = FindObjectOfType<CharacterController>().gameObject;
        }
        private void Update()
        {
            var distanceToPlayer = Vector3.Distance(_transform.position, _player.transform.position);
            if (distanceToPlayer < detectionDistance && distanceToPlayer > closeEnoughDistance)
            {
                var vectorToPlayer = (_player.transform.position - _transform.position).normalized;
                _transform.Translate(vectorToPlayer * speed * Time.deltaTime);
            }
        }
        private void LateUpdate()
        {
            _transform.LookAt(_player.transform);
        }
    }
}
