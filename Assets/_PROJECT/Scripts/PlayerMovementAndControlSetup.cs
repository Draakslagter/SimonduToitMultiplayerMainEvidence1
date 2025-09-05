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
    
    [Header ("Interaction")]
    private Transform _characterTransform;
    private Transform _interactionTransform;
    private Rigidbody _interactionRb;
    private bool _holdingObject;
    
    [Header ("Pause")]
    public UnityEvent triggerPauseMenu;
    
    [Header ("Destroy")]
    public UnityEvent triggerDestroy;

    private void Awake()
    {
        
        _jumpToken = maxJumpToken;
        _characterInputMap = new CharacterInput();

        _characterInputMap.Enable();
        
        if (_characterRb == null)
        {
            _characterRb = GetComponent<Rigidbody>();
        }
        if (_characterTransform == null)
        {
            _characterTransform = transform;
        }
    }
    private void Start()
    {
        triggerPauseMenu.AddListener(PauseMenu.Instance.PauseGame);
    }

    private void OnDisable()
    {
        triggerDestroy.Invoke();
    }

    private void FixedUpdate()
    {
       _characterRb.transform.Translate(_movementVector * (speedMultiplier * Time.fixedDeltaTime));
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementVector = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
      
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        Debug.Log("We Picked Up");
        if (_interactionTransform == null || _holdingObject) return;
        _interactionTransform.SetParent(_characterTransform);
        _interactionRb.constraints = RigidbodyConstraints.FreezePosition;
        _holdingObject = true;
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        Debug.Log("We Dropped");
        Debug.Log(_holdingObject);
        Debug.Log(_interactionTransform);
        if (_interactionTransform == null || _holdingObject == false) return;
        _interactionRb.constraints = RigidbodyConstraints.None;
        _interactionRb.constraints = RigidbodyConstraints.FreezePositionX|RigidbodyConstraints.FreezePositionZ;
        _interactionTransform.parent = null;
        _holdingObject = false;
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

    private void OnCollisionEnter(Collision other)
    {
        if (!other.transform.CompareTag("Interactable") || _holdingObject) return;
        _interactionTransform = other.transform;
        _interactionRb = _interactionTransform.GetComponent<Rigidbody>();
        Debug.Log(_interactionTransform);
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Interactable") && _holdingObject == false)
        {
            _interactionTransform = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
