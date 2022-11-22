using Gameplay;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Generated.PlayerInput;

namespace Character
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(ObjectPool))]
    public class PlayerController : MonoBehaviour, IProcessMovement, IProcessFireWeaponCommand
    {
        public float speed = 7f;
        public float timeBetweenShotsInSeconds = 0.25f;
        public Vector2 initialMovementVector = Vector2.zero;
        public Transform firingOrigin;
        public ParticleSystem projectileParticles;
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
            _transform = transform;
            _playerInput = new PlayerInput();
            _playerInput.GamePlay.Move.started += HandleMovement;
            _playerInput.GamePlay.Move.performed += HandleMovement;
            _playerInput.GamePlay.Move.canceled += HandleMovement;
            _playerInput.GamePlay.Fire.started += HandleFireWeaponCommand;
            _playerInput.GamePlay.Fire.performed += HandleFireWeaponCommand;
            _playerInput.GamePlay.Fire.canceled += HandleFireWeaponCommand;
        }

        public void Reset()
        {
            if (_transform) _transform.position = Vector3.zero;
        }

        private void Start() => _characterController = GetComponent<CharacterController>();
        private void Update()
        {
            _characterController.Move(PlayerMoveVector * Time.deltaTime);
            if (PlayerMoveVector != Vector3.zero)
            {
                _transform.rotation = Quaternion.LookRotation(new Vector3(MoveInput.x, MoveInput.y, 0));
                if (_transform.eulerAngles.y == 0f)
                {
                    var transformEulerAngles = _transform.eulerAngles;
                    _transform.eulerAngles = new Vector3(transformEulerAngles.x, 90f, transformEulerAngles.z);
                }
            }
            FireDirection = new Vector3(FireInput.x, FireInput.y, 0).normalized;
            if (FireInput != Vector2.zero && Time.time - _timeLastFired > timeBetweenShotsInSeconds) Fire();
        }
        private void OnEnable()
        {
            _playerInput.Enable();
            _timeLastFired = Time.time - timeBetweenShotsInSeconds;
            if (initialMovementVector != Vector2.zero)
            {
                MoveInput = new Vector2(-initialMovementVector.x, initialMovementVector.y);
                PlayerMoveVector = new Vector3(MoveInput.x, MoveInput.y, 0) * speed;
            }
        }
        private void OnDisable() => _playerInput.Disable();

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
            if (firingOrigin == null) return;
            if (projectileParticles == null) return;
            if (FireInput == Vector2.zero) return;

            _timeLastFired = Time.time;
            var fireDirection = new Vector3(FireInput.x, FireInput.y, 0);
            var directionWithBuffer = _transform.position + fireDirection.normalized * 3f;
            firingOrigin.position = directionWithBuffer;
            firingOrigin.rotation = Quaternion.LookRotation(new Vector3(FireInput.x, FireInput.y, 0));
            projectileParticles.Emit(1);
        }
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
