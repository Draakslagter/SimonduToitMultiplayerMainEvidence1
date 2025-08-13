using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovementAndControlSetup : MonoBehaviour
{
    private CharacterInput _characterInputMap;

    private Rigidbody _characterRb;
    private Vector3 _movementVector;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float jumpMultiplier;

    private void Awake()
    {
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
        _characterRb.AddForce(Vector3.up * jumpMultiplier, ForceMode.Impulse);
    }

}
