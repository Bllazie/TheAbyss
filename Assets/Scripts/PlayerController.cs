using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{

    private PlayerInputActions _playerInputActions;
    private Vector2 _moveInput;
    private CharacterController _characterController;
    private float _verticalVelocity;
    
    [SerializeField] private Vector3 _movementDirection;
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _stamina;

    [SerializeField] private float _maxStamina = 10f;
    [SerializeField] private float _staminaRunDrainPerSecond = 1f;
    [SerializeField] private float _staminaJumpDrainPerSecond = 2f;
    [SerializeField] private float _staminaRegenPerSecond = 1f;
    private bool _isRunning;

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Jump.performed += context => JumpHandler();
        _playerInputActions.Player.Movement.performed += context => _moveInput = context.ReadValue<Vector2>();
        _playerInputActions.Player.Movement.canceled += context => _moveInput = Vector2.zero;

        _playerInputActions.Player.Run.performed += context => _isRunning = true;
        _playerInputActions.Player.Run.canceled += context => _isRunning = false;
    }
    private void OnEnable()
    {
        _playerInputActions.Enable();
    }
    private void OnDisable()
    {
        _playerInputActions.Disable();
    }
    private void JumpHandler()
    {
        if (_characterController.isGrounded && _stamina > _staminaJumpDrainPerSecond)
        {
            _verticalVelocity = _jumpForce;
            _stamina -= _staminaJumpDrainPerSecond;
        }
        else
        {
            // вывод в hud сообщения НЕТ СТАМИНЫ
        }
    }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    private void Update()
    {

        Vector3 move = transform.forward * _moveInput.y + transform.right * _moveInput.x;

        float currentSpeed = _walkSpeed;
        if (_isRunning && _stamina > 0 && move.magnitude > 0.1f)
        {
            currentSpeed = _runSpeed;
            _stamina -= _staminaRunDrainPerSecond * Time.deltaTime;
            if (_stamina < 0) _stamina = 0;
        }
        else
        {
            if (_stamina < _maxStamina)
            {
                currentSpeed = _walkSpeed;
                _stamina += _staminaRegenPerSecond * Time.deltaTime;
                if(_stamina > _maxStamina) _stamina = _maxStamina;
            }
        }
        if (_characterController.isGrounded)
        {
            if(_verticalVelocity<0) _verticalVelocity = -1f;
        }
        else
        {
            _verticalVelocity -= 9.81f * Time.deltaTime;
        }
        move.y = _verticalVelocity;
        _characterController.Move(move * currentSpeed * Time.deltaTime);
    }
}
