using System;
using Combat;
using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(ObjectPool))]
    public class PlayerController : MonoBehaviour, ICanDie
    {
        public float speed = 7f;
        public GameObject bulletPrefab;
        public float timeBetweenShotsInSeconds = 0.25f;
        public ObjectPool bulletPool;
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
            bulletPool ??= GetComponent<ObjectPool>();
            if (bulletPool == null)
            {
                enabled = false;
                throw new NullReferenceException("bullet pool required");
            }
        }
        private void Update()
        {
            if (Gamepad.current == null) return;
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

        private void OnCollisionEnter(Collision collision) => Died?.Invoke(gameObject);

        public event Action<GameObject> Died;

        private void Move()
        {
            _characterController.Move(PlayerMoveVector * Time.deltaTime);
            if (PlayerMoveVector != Vector3.zero)
                _transform.rotation = Quaternion.LookRotation(new Vector3(MoveInput.x, MoveInput.y, 0));
        }
        private GameObject CreatePoolObject()
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullet.layer = gameObject.layer;
            return bullet;
        }
        private void Fire()
        {
            var bullet = GetBulletFromPool();
            bullet.transform.position = gameObject.transform.position + FireDirection * 3f;
            if (bullet.TryGetComponent<Projectile>(out var projectile))
            {
                projectile.firingDirectionNormalized = FireDirection;
                projectile.ImpactHasOccurred += impact => bulletPool.RecyclePoolObject(impact.Impacter);
            }
            bullet.SetActive(true);
            _timeLastFired = Time.time;
        }
        private GameObject GetBulletFromPool() => bulletPool.GetPoolObject();
    }
}
