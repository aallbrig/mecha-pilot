using System;
using Combat;
using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Generated.PlayerInput;

namespace Character
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(ObjectPool))]
    [RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput))]
    public class PlayerController : MonoBehaviour, ICanDie, IProcessMovement, IProcessFireWeaponCommand
    {
        public float speed = 7f;
        public GameObject bulletPrefab;
        public float timeBetweenShotsInSeconds = 0.25f;
        public ObjectPool bulletPool;
        private CharacterController _characterController;
        private PlayerInput _playerInput;
        private float _timeLastFired;
        private Transform _transform;

        public Vector3 PlayerMoveVector { get; private set; }

        public Vector3 FireDirection { get; private set; }

        public Vector2 MoveInput { get; private set; }

        public Vector2 FireInput { get; private set; }

        private void Awake()
        {
            _playerInput = new PlayerInput();
            _playerInput.GamePlay.Move.started += HandleMovement;
            _playerInput.GamePlay.Move.performed += HandleMovement;
            _playerInput.GamePlay.Move.canceled += HandleMovement;
            _playerInput.GamePlay.Fire.started += HandleFireWeaponCommand;
            _playerInput.GamePlay.Fire.performed += HandleFireWeaponCommand;
            _playerInput.GamePlay.Fire.canceled += HandleFireWeaponCommand;
        }

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
            // if (Gamepad.current == null) return;
            _characterController.Move(PlayerMoveVector * Time.deltaTime);
            if (PlayerMoveVector != Vector3.zero)
                _transform.rotation = Quaternion.LookRotation(new Vector3(MoveInput.x, MoveInput.y, 0));
            // MoveInput = Gamepad.current.leftStick.ReadValue();
            // FireInput = Gamepad.current.rightStick.ReadValue();
            // player is facing camera, so x is backwards -- correcting now
            // MoveInput = new Vector2(-MoveInput.x, MoveInput.y);
            // FireInput = new Vector2(-FireInput.x, FireInput.y);
            // PlayerMoveVector = new Vector3(MoveInput.x, MoveInput.y, 0) * speed;
            // FireDirection = new Vector3(FireInput.x, FireInput.y, 0).normalized;
            // Move();
            FireDirection = new Vector3(FireInput.x, FireInput.y, 0).normalized;
            if (FireInput != Vector2.zero && Time.time - _timeLastFired > timeBetweenShotsInSeconds) Fire();
        }
        private void OnEnable() => _playerInput.Enable();
        private void OnDisable() => _playerInput.Disable();

        private void OnCollisionEnter(Collision collision) => Died?.Invoke(gameObject);

        public event Action<GameObject> Died;

        public void HandleFireWeaponCommand(InputAction.CallbackContext context)
        {
            FireInput = context.ReadValue<Vector2>();
            FireInput = new Vector2(-FireInput.x, FireInput.y);
        }

        public void HandleMovement(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
            MoveInput = new Vector2(-MoveInput.x, MoveInput.y);
            PlayerMoveVector = new Vector3(MoveInput.x, MoveInput.y, 0) * speed;
        }
        private void Move()
        {
            _characterController.Move(PlayerMoveVector * Time.deltaTime);
            if (PlayerMoveVector != Vector3.zero)
                _transform.rotation = Quaternion.LookRotation(new Vector3(MoveInput.x, MoveInput.y, 0));
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

    public interface IProcessMovement
    {
        public void HandleMovement(InputAction.CallbackContext context);
    }

    public interface IProcessFireWeaponCommand
    {
        public void HandleFireWeaponCommand(InputAction.CallbackContext context);
    }
}
