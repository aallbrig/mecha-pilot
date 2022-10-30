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
        private Transform _transform;

        public Vector3 PlayerMoveVector { get; private set; }

        public Vector3 FireDirection { get; private set; }

        public Vector2 MoveInput { get; private set; }

        public Vector2 FireInput { get; private set; }

        private void Start()
        {
            _transform = transform;
            _timeLastFired = Time.time - timeBetweenShotsInSeconds;
            _characterController = GetComponent<CharacterController>();
            for (var i = 0; i < _numberOfBullets; i++)
                _bulletPool.Add(CreatePoolObject());
        }
        private void Update()
        {
            MoveInput = Gamepad.current.leftStick.ReadValue();
            FireInput = Gamepad.current.rightStick.ReadValue();
            // player is facing camera, so x is backwards -- correcting now
            MoveInput = new Vector2(-MoveInput.x, MoveInput.y);
            FireInput = new Vector2(-FireInput.x, FireInput.y);
            PlayerMoveVector = new Vector3(MoveInput.x, MoveInput.y, 0) * speed;
            FireDirection = new Vector3(FireInput.x, FireInput.y, 0).normalized;
            Move();
            if (FireInput != Vector2.zero && Time.time - _timeLastFired > timeBetweenShotsInSeconds) Fire();

        }
        private void Move()
        {
            _transform.rotation = Quaternion.LookRotation(new Vector3(MoveInput.x, MoveInput.y, 0));
            _characterController.Move(PlayerMoveVector * Time.deltaTime);
        }
        private GameObject CreatePoolObject()
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            return bullet;
        }
        private void Fire()
        {
            var bullet = GetBulletFromPool();
            bullet.transform.position = gameObject.transform.position + FireDirection * 1.1f;
            if (bullet.TryGetComponent<Projectile>(out var projectile))
            {
                projectile.firingDirectionNormalized = FireDirection;
                projectile.initialSpeedVector = PlayerMoveVector;
            }
            bullet.SetActive(true);
            _timeLastFired = Time.time;
        }
        private GameObject GetBulletFromPool()
        {
            foreach (var bullet in _bulletPool)
                if (bullet.activeSelf == false)
                    return bullet;
            var newBullet = CreatePoolObject();
            _bulletPool.Add(newBullet);
            return newBullet;
        }
    }
}
