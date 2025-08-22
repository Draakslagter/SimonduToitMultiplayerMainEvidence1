using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovementAndControlSetup : MonoBehaviour
{
    
        
    [Header ("Control")]
    private CharacterInput _characterInputMap;
    
    [Header ("Movement")]
    private Rigidbody _characterRb;
    private Vector3 _movementVector;
    [SerializeField] private float speedMultiplier, jumpMultiplier, dashMultiplier;
    private int _jumpToken;
    [SerializeField] private int maxJumpToken;
    
    [Header ("Jump")]
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    
    [Header ("Pause")]
    public UnityEvent triggerPauseMenu;

    private void Awake()
    {
        
        _jumpToken = maxJumpToken;
        _characterInputMap = new CharacterInput();

        _characterInputMap.Enable();
        
        if (_characterRb == null)
        {
            _characterRb = GetComponent<Rigidbody>();
        }
    }
    private void Start()
    {
        triggerPauseMenu.AddListener(PauseMenu.Instance.PauseGame);
    }

    private void FixedUpdate()
    {
       _characterRb.transform.Translate(_movementVector * (speedMultiplier * Time.fixedDeltaTime));
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementVector = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
      
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("We Interacted");
    }

    public void OnPause(InputAction.CallbackContext context)
    {
       triggerPauseMenu.Invoke();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _characterRb.AddForce(_movementVector * dashMultiplier, ForceMode.Impulse);
        }
        else if (context.canceled)
        {
            _characterRb.linearVelocity = Vector3.zero;
        }
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
        _characterRb.linearVelocity = new Vector3(0, 1, 0);
        var jumpVector = new Vector3(0, jumpMultiplier, 0);
        _characterRb.AddForce(jumpVector, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
