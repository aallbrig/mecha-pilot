using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Generated.PlayerInput;

namespace Player
{
    public class PlayerInputActions : MonoBehaviour
    {
        public PlayerController playerController;
        private PlayerInput _playerInput;
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

        private void OnEnable() => _playerInput.Enable();
        private void OnDisable() => _playerInput.Disable();
        private void HandleFireWeaponCommand(InputAction.CallbackContext obj) =>
            playerController.HandleFireWeaponCommand(obj);

        private void HandleMovement(InputAction.CallbackContext obj) =>
            playerController.HandleMovement(obj);
    }
}
