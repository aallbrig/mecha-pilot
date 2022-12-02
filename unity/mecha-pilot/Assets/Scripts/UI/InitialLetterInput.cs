using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using PlayerInput = Generated.PlayerInput;

namespace UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class InitialLetterInput : MonoBehaviour
    {
        private static readonly string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ ";
        private TMP_InputField _inputField;
        private Selectable _nextSelect;
        private PlayerInput _playerInput;
        private void Awake()
        {
            _inputField = GetComponent<TMP_InputField>();
            _playerInput = new PlayerInput();
        }
        private void HandleUINavigate(InputAction.CallbackContext ctx)
        {
            var input = ctx.ReadValue<Vector2>();
            Debug.Log($"Handle UI navigate {input}");
            if (input == Vector2.up)
            {
                var currentIndex = Alphabet.IndexOf(_inputField.text, StringComparison.Ordinal);
                var nextIndex = currentIndex == 0 ? Alphabet.Length - 1 : currentIndex - 1;
                _inputField.text = Alphabet[nextIndex].ToString();
            }
            else if (input == Vector2.down)
            {
                var currentIndex = Alphabet.IndexOf(_inputField.text, StringComparison.Ordinal);
                var nextIndex = currentIndex == Alphabet.Length - 1 ? 0 : currentIndex + 1;
                _inputField.text = Alphabet[nextIndex].ToString();
            }
            else if (input == Vector2.left && _inputField.navigation.selectOnLeft)
            {
                _nextSelect = _inputField.navigation.selectOnLeft;
            }
            else if (input == Vector2.right && _inputField.navigation.selectOnRight)
            {
                _nextSelect = _inputField.navigation.selectOnRight;
            }
        }
        public void OnInputSelect()
        {
            Debug.Log($"{name} selected");
            _playerInput.GamePlay.Move.performed += HandleUINavigate;
            _playerInput.GamePlay.Move.canceled += NavigateIfNextSelectable;
            _playerInput.Enable();
        }
        public void OnInputDeselect()
        {
            Debug.Log($"{name} deselected");
            _playerInput.Disable();
            _playerInput.GamePlay.Move.performed -= HandleUINavigate;
            _playerInput.GamePlay.Move.canceled -= NavigateIfNextSelectable;
        }
        private void NavigateIfNextSelectable(InputAction.CallbackContext obj)
        {
            if (_nextSelect)
            {
                _nextSelect.Select();
                _nextSelect = null;
            }
        }
    }
}
