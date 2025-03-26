using UnityEngine;

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool analogMovement;

        // Nou paràmetre per crouch
        [Header("Controls personalitzats")]
        public bool crouchPressed;

        public void OnMove(UnityEngine.InputSystem.InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(UnityEngine.InputSystem.InputValue value)
        {
            LookInput(value.Get<Vector2>());
        }

        public void OnJump(UnityEngine.InputSystem.InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(UnityEngine.InputSystem.InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void OnCrouch(UnityEngine.InputSystem.InputValue value)
        {
            CrouchInput(value.isPressed);
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void CrouchInput(bool newCrouchState)
        {
            crouchPressed = newCrouchState;
        }
    }
}
