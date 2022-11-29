using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour, IProcessMovement, IProcessFireWeaponCommand
    {
        public float speed = 7f;
        public float timeBetweenShotsInSeconds = 0.25f;
        public Vector2 initialMovementVector = Vector2.zero;
        public Transform firingOrigin;
        public ParticleSystem projectileParticles;
        public UnityEvent onWeaponFired;
        private CharacterController _characterController;
        private Vector3 _startFireOriginPosition;
        private Quaternion _startFireOriginRotation;
        private float _timeLastFired;
        private Transform _transform;

        public Vector3 PlayerMoveVector { get; private set; }

        public Vector3 FireDirection { get; private set; }

        public Vector2 MoveInput { get; private set; }

        public Vector2 FireInput { get; private set; }

        public Vector2 LastMoveInput { get; set; }

        private void Awake() => _transform = transform;

        public void Reset()
        {
            if (_transform) _transform.position = Vector3.zero;
        }

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _startFireOriginPosition = firingOrigin.localPosition;
            _startFireOriginRotation = firingOrigin.localRotation;
        }
        private void Update()
        {
            _characterController.Move(PlayerMoveVector * Time.deltaTime);
            var moveInputConsidered = MoveInput == Vector2.zero ? LastMoveInput : MoveInput;
            _transform.rotation = Quaternion.LookRotation(new Vector3(moveInputConsidered.x, moveInputConsidered.y, 0));
            if (PlayerMoveVector != Vector3.zero)
                if (_transform.eulerAngles.y == 0f)
                {
                    var transformEulerAngles = _transform.eulerAngles;
                    _transform.eulerAngles = new Vector3(transformEulerAngles.x, 90f, transformEulerAngles.z);
                }
            FireDirection = new Vector3(FireInput.x, FireInput.y, 0).normalized;
            if (FireInput != Vector2.zero)
            {
                firingOrigin.rotation = Quaternion.LookRotation(new Vector3(FireInput.x, FireInput.y, 0));
            }
            else if (firingOrigin.localPosition != _startFireOriginPosition ||
                     firingOrigin.localRotation != _startFireOriginRotation)
            {
                firingOrigin.localPosition = _startFireOriginPosition;
                firingOrigin.localRotation = _startFireOriginRotation;
            }
            if (FireInput != Vector2.zero && Time.time - _timeLastFired > timeBetweenShotsInSeconds) Fire();
        }
        private void OnEnable()
        {
            _timeLastFired = Time.time - timeBetweenShotsInSeconds;
            if (initialMovementVector != Vector2.zero)
            {
                MoveInput = new Vector2(-initialMovementVector.x, initialMovementVector.y);
                LastMoveInput = MoveInput.normalized * 0.3f;
                PlayerMoveVector = new Vector3(MoveInput.x, MoveInput.y, 0) * speed;
            }
        }

        public void HandleFireWeaponCommand(InputAction.CallbackContext context)
        {
            FireInput = context.ReadValue<Vector2>();
            FireInput = new Vector2(-FireInput.x, FireInput.y);
        }

        public void HandleMovement(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
            if (MoveInput != Vector2.zero)
            {
                MoveInput = new Vector2(-MoveInput.x, MoveInput.y);
                LastMoveInput = MoveInput.normalized * 0.3f;
                PlayerMoveVector = new Vector3(MoveInput.x, MoveInput.y, 0) * speed;
            }
            else
            {
                PlayerMoveVector = new Vector3(LastMoveInput.x, LastMoveInput.y, 0) * speed;
            }
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
            projectileParticles.Emit(1);

            onWeaponFired?.Invoke();
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
