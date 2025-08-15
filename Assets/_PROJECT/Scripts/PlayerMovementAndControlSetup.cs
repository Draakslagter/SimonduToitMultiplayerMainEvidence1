using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovementAndControlSetup : MonoBehaviour
{
    [Header ("Control")]
    private CharacterInput _characterInputMap;
    
    [Header ("Movement")]
    private Rigidbody _characterRb;
    private Vector3 _movementVector;
    [SerializeField] private float speedMultiplier, jumpMultiplier;
    private int _jumpToken;
    [SerializeField] private int maxJumpToken;
    
    [Header ("Jump")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        _jumpToken = maxJumpToken;
        _characterInputMap = new CharacterInput();

        _characterInputMap.Enable();

        // characterInputMap.PlayerMap.Jump.performed += OnJump;
        // characterInputMap.PlayerMap.Jump.canceled -= OnJump;
        //
        // characterInputMap.PlayerMap.Attack.performed += OnAttack;
        // characterInputMap.PlayerMap.Attack.canceled -= OnAttack;
        //
        // characterInputMap.PlayerMap.Pause.performed += OnPause;
        // characterInputMap.PlayerMap.Pause.canceled -= OnPause;
        //
        // characterInputMap.PlayerMap.Interact.performed += OnInteract;
        // characterInputMap.PlayerMap.Interact.canceled -= OnInteract;
        //
        // characterInputMap.PlayerMap.Movement.performed += x => OnPlayerMove(x.ReadValue<Vector2>());
        _characterInputMap.PlayerMap.Movement.canceled += x => OnStopMove(x.ReadValue<Vector2>());

        if (_characterRb == null)
        {
            _characterRb = GetComponent<Rigidbody>();
        }
    }

    private void OnDisable()
    {
        // characterInputMap.PlayerMap.Movement.performed -= x => OnPlayerMove(x.ReadValue<Vector2>());
        _characterInputMap.PlayerMap.Movement.canceled -= x => OnStopMove(x.ReadValue<Vector2>());
    }

    private void FixedUpdate()
    { 
       _characterRb.transform.Translate(_movementVector * (speedMultiplier * Time.fixedDeltaTime));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movementVector.x = context.ReadValue<Vector2>().x;
        _movementVector.z = context.ReadValue<Vector2>().y;
    }

    private void OnStopMove(Vector2 incomingVector2)
    {
        _movementVector.x = 0;
        _movementVector.z = 0;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("We Interacted");
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        Debug.Log("We Paused");
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        Debug.Log("We Attacked");
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        var groundArray = Physics.OverlapSphere(groundCheckTransform.position, groundCheckRadius, groundLayer);
        if (groundArray.Length == 0)
        {
            if (_jumpToken == 0)
            {
                return;
            }

            _jumpToken--;
        }
        else
        {
            _jumpToken = maxJumpToken;
        }

        
        var jumpVector = new Vector3(0, jumpMultiplier, 0);
        _characterRb.AddForce(jumpVector, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
