using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovementAndControlSetup : MonoBehaviour
{
    
        
    [Header ("Control")]
    private CharacterInput _characterInputMap;
    private bool _gameStarted;
    private PlayerAudio _playerAudio;
    
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
    public Transform pickUpTransform;
    [SerializeField] private float pickUpRadius;
    
    public Transform interactionTransform;
    public Rigidbody interactionRb;
    private bool _holdingObject;

    [SerializeField] private LayerMask playerLayerMask;

    [Header("Spawn Item")] 
    public UnityEvent<PlayerMovementAndControlSetup> triggerSpawnItem;
    
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

        if (_playerAudio == null)
        {
            _playerAudio = GetComponent<PlayerAudio>();
        }
    }
    private void Start()
    {
        triggerPauseMenu.AddListener(PauseMenu.Instance.PauseGame);
        triggerSpawnItem.AddListener(BlockSpawner.Instance.SpawnBlock);
        PlayerLobbyManager.Instance.triggerGameStart.AddListener(CheckGameStart);
    }

    private void OnDisable()
    {
        triggerDestroy.Invoke();
    }

    private void FixedUpdate()
    {
        if (!_gameStarted) return;
       _characterRb.transform.Translate(_movementVector * (speedMultiplier * Time.fixedDeltaTime));
       _playerTransform.eulerAngles = _movementVector;
       if (_movementVector is { x: 0, z: 0 }) return;
       pickUpTransform.position = new Vector3(_playerTransform.position.x + _movementVector.x, _playerTransform.position.y,  _playerTransform.position.z + _movementVector.z );
    if (!_holdingObject || !interactionTransform) return;
       if (!Physics.Raycast(interactionTransform.position,
               (_playerTransform.position - interactionTransform.position).normalized, out var hit, Mathf.Infinity,
               playerLayerMask)) return;
       interactionTransform.forward = hit.normal;
       interactionTransform.position = new Vector3(hit.point.x, hit.point.y + interactionTransform.localScale.y * 0.5f, hit.point.z);
       interactionTransform.position += interactionTransform.forward * (interactionTransform.localScale.z);
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        _movementVector = new Vector3(context.ReadValue<Vector2>().x, 0, context.ReadValue<Vector2>().y);
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        CheckPickUp();
        if (interactionTransform == null || _holdingObject) return;
        interactionTransform.SetParent(pickUpTransform);
        interactionRb.constraints = RigidbodyConstraints.None;
        interactionRb.constraints = RigidbodyConstraints.FreezePosition;
        _holdingObject = true;
        _playerAudio.PlayClip(1);
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (interactionTransform == null || _holdingObject == false) return;
        interactionRb.constraints = RigidbodyConstraints.FreezeAll;
        interactionTransform.parent = null;
        _holdingObject = false;
        interactionTransform.gameObject.layer = 3;
        interactionTransform = null;
        interactionRb = null;
    }

    public void OnCreate(InputAction.CallbackContext context)
    {
        if (!_gameStarted) return;
        CheckPickUp();
        if (interactionTransform != null || _holdingObject) return;
        triggerSpawnItem.Invoke(this);
        _holdingObject = true;
        _playerAudio.PlayClip(1);
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
        _playerAudio.PlayClip(0);
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
        _playerAudio.PlayClip(0);
        _characterRb.linearVelocity = new Vector3(0, 1, 0);
        var jumpVector = new Vector3(0, jumpMultiplier, 0);
        _characterRb.AddForce(jumpVector, ForceMode.Impulse);
    }

    private void CheckPickUp()
    {
        var interactableArray = Physics.OverlapSphere(pickUpTransform.position, pickUpRadius, groundLayer);
        if (interactableArray.Length != 1 || _holdingObject) return;
        interactionTransform = interactableArray[0].transform;
        interactionRb = interactionTransform.gameObject.GetComponent<Rigidbody>();
        interactionTransform.gameObject.layer = 6;
    }

    private void CheckGameStart(bool gameStart)
    {
        Debug.Log("Player Check Game Start" + gameStart);
        _gameStarted =  gameStart;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(pickUpTransform.position, pickUpRadius);
    }
}
