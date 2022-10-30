using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        public Vector2 MoveVector { get; private set; }

        public Vector2 FireVector { get; private set; }

        public Vector2 MoveInput { get; private set; }

        public Vector2 FireInput { get; private set; }

        private void Update()
        {
            MoveInput = Gamepad.current.leftStick.ReadValue();
            FireInput = Gamepad.current.rightStick.ReadValue();
        }
    }
}
