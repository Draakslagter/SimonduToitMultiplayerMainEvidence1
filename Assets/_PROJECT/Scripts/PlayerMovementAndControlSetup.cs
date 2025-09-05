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
    private Transform _playerTransform;
    [SerializeField] private Transform pickUpTransform;
    [SerializeField] private float pickUpRadius;
    
    private Transform _interactionTransform;
    private Rigidbody _interactionRb;
    private bool _holdingObject;

    [SerializeField] private LayerMask playerLayerMask;
    
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

        if (_playerTransform == null)
        {
            _playerTransform = GetComponent<Transform>();
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
       if (_movementVector is { x: 0, z: 0 }) return;
       
       pickUpTransform.position = new Vector3(_playerTransform.position.x + _movementVector.x, _playerTransform.position.y,  _playerTransform.position.z + _movementVector.z );
    
       if (!Physics.Raycast(_interactionTransform.position,
               (_playerTransform.position - _interactionTransform.position).normalized, out var hit, Mathf.Infinity,
               playerLayerMask)) return;
       _interactionTransform.forward = hit.normal;
       _interactionTransform.position = new Vector3(hit.point.x, hit.point.y + _interactionTransform.localScale.y * 0.5f, hit.point.z);
       _interactionTransform.position += _interactionTransform.forward * (_interactionTransform.localScale.z);
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementVector = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        CheckPickUp();
       
        if (_interactionTransform == null || _holdingObject) return;
        _interactionTransform.SetParent(pickUpTransform);
        _interactionRb.constraints = RigidbodyConstraints.FreezePosition;
        _holdingObject = true;
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (_interactionTransform == null || _holdingObject == false) return;
        _interactionRb.constraints = RigidbodyConstraints.None;
        _interactionRb.constraints = RigidbodyConstraints.FreezePositionX|RigidbodyConstraints.FreezePositionZ;
        _interactionTransform.parent = null;
        _holdingObject = false;
        _interactionTransform.gameObject.layer = 3;
        _interactionTransform = null;
        _interactionRb = null;
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

    private void CheckPickUp()
    {
        var interactableArray = Physics.OverlapSphere(pickUpTransform.position, pickUpRadius, groundLayer);
        if (interactableArray.Length != 1 || _holdingObject) return;
        _interactionTransform = interactableArray[0].transform;
        _interactionRb = _interactionTransform.gameObject.GetComponent<Rigidbody>();
        _interactionTransform.gameObject.layer = 6;
        
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pickUpTransform.position, pickUpRadius);
    }
}
