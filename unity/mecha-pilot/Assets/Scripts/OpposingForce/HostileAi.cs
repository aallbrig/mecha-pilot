using System;
using UnityEngine;

namespace OpposingForce
{
    public class HostileAi : MonoBehaviour
    {
        private GameObject _player;
        private Transform _transform;
        private void Start()
        {
            _transform = transform;
            _player = FindObjectOfType<CharacterController>().gameObject;
        }
        private void Update()
        {
            _transform.LookAt(_player.transform);
        }
    }
}
