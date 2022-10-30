using System.Collections.Generic;
using Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public float speed = 7f;
        public GameObject bulletPrefab;
        public float timeBetweenShotsInSeconds = 0.25f;
        private readonly List<GameObject> _bulletPool = new List<GameObject>();
        private readonly int _numberOfBullets = 30;
        private CharacterController _characterController;
        private float _timeLastFired;

        public Vector3 MoveVector { get; private set; }

        public Vector3 FireVector { get; private set; }

        public Vector2 MoveInput { get; private set; }

        public Vector2 FireInput { get; private set; }

        private void Start()
        {
            _timeLastFired = Time.time - timeBetweenShotsInSeconds;
            _characterController = GetComponent<CharacterController>();
            for (var i = 0; i < _numberOfBullets; i++)
            {
                var bullet = Instantiate(bulletPrefab);
                bullet.SetActive(false);
                _bulletPool.Add(bullet);
            }
        }
        private void Update()
        {
            MoveInput = Gamepad.current.leftStick.ReadValue();
            MoveVector = new Vector3(MoveInput.x, MoveInput.y, 0) * speed;
            FireInput = Gamepad.current.rightStick.ReadValue();
            FireVector = new Vector3(FireInput.x, FireInput.y, 0).normalized;
            _characterController.Move(MoveVector * Time.deltaTime);

            if (FireInput != Vector2.zero && Time.time - _timeLastFired > timeBetweenShotsInSeconds) Fire();
        }
        private void Fire()
        {
            var bullet = GetBulletFromPool();
            bullet.transform.position = gameObject.transform.position + FireVector * 1.1f;
            if (bullet.TryGetComponent<Projectile>(out var projectile))
                projectile.normalizedVector = FireVector;
            bullet.SetActive(true);
            _timeLastFired = Time.time;
        }
        private GameObject GetBulletFromPool()
        {
            for (var i = 0; i < _bulletPool.Count; i++)
            {
                var bullet = _bulletPool[i];
                if (bullet.activeSelf == false)
                    return bullet;
            }
            var newBullet = Instantiate(bulletPrefab);
            newBullet.SetActive(false);
            _bulletPool.Add(newBullet);
            return newBullet;
        }
    }
}
