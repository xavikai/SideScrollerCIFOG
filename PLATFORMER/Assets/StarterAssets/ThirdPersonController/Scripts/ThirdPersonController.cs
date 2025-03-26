using UnityEngine;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Configuració de moviment")]
        public float MoveSpeed = 2.0f;
        public float SprintSpeed = 5.335f;
        public float RotationSmoothTime = 0.12f;
        public float SpeedChangeRate = 10.0f;
        public float crouchSpeed = 1.0f;

        [Header("Salt i gravetat")]
        public float JumpHeight = 1.2f;
        public float Gravity = -15.0f;
        public float JumpTimeout = 0.50f;
        public float FallTimeout = 0.15f;
        public bool enableDoubleJump = true;

        [Header("Crouch Settings")]
        public bool enableCrouch = true;
        private bool isCrouching = false;

        [Header("Grounded")]
        public bool Grounded = true;
        public float GroundedOffset = -0.14f;
        public float GroundedRadius = 0.28f;
        public LayerMask GroundLayers;

        [Header("Càmera")]
        public GameObject CinemachineCameraTarget;
        public float TopClamp = 70.0f;
        public float BottomClamp = -30.0f;

        private float _speed;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;
        private int remainingJumps;

        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private int _animIDSpeed;
        private int _animIDCrouch;
        private int _animIDDoubleJump;
        private int _animIDDeath;

        private void Awake()
        {
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();

            AssignAnimationIDs();
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
            remainingJumps = enableDoubleJump ? 2 : 1;
        }

        private void Update()
        {
            GroundedCheck();
            HandleCrouch();
            JumpAndGravity();
            Move();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDCrouch = Animator.StringToHash("Crouch");
            _animIDDoubleJump = Animator.StringToHash("DoubleJump");
            _animIDDeath = Animator.StringToHash("Die");
        }

        private void GroundedCheck()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            if (Grounded)
            {
                remainingJumps = enableDoubleJump ? 2 : 1;
            }
        }

        private void HandleCrouch()
        {
            if (enableCrouch && _input.crouchPressed)
            {
                isCrouching = !isCrouching;
                _animator.SetBool(_animIDCrouch, isCrouching);
                _input.crouchPressed = false;
            }
        }

        private void Move()
        {
            float targetSpeed = isCrouching ? crouchSpeed : (_input.sprint ? SprintSpeed : MoveSpeed);

            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;
            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            if (_input.move != Vector2.zero)
            {
                float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _verticalVelocity, RotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f) * Vector3.forward;
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            _animator.SetFloat(_animIDSpeed, _speed);

            if (isCrouching && _speed > 0.1f)
            {
                _animator.SetBool("IsCrouchWalking", true);
            }
            else
            {
                _animator.SetBool("IsCrouchWalking", false);
            }
        }

        private void JumpAndGravity()
        {
            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;
                if (_verticalVelocity < 0.0f) _verticalVelocity = -2f;

                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    remainingJumps--;
                }

                if (_jumpTimeoutDelta >= 0.0f) _jumpTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (_input.jump && enableDoubleJump && remainingJumps > 0)
                {
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                    remainingJumps--;
                    _animator.SetTrigger(_animIDDoubleJump);
                }

                if (_verticalVelocity < _terminalVelocity) _verticalVelocity += Gravity * Time.deltaTime;
                _input.jump = false;
            }
        }

        public void PlayDeathAnimation()
        {
            _animator.SetTrigger(_animIDDeath);
        }
    }
}
