using UnityEngine;
using UnityEngine.InputSystem;

public class WaterMovement : MonoBehaviour
{
    private bool _gameStart;
    private Rigidbody _waterRb;
    [SerializeField] private float _waterSpeedMultiplier;
    
    private void Awake()
    {
        _waterRb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        PlayerLobbyManager.Instance.triggerWaterMovement.AddListener(StartMovingWater);
    }

    private void StartMovingWater(bool playerJoined)
    {
        Debug.Log(playerJoined);
        _gameStart = playerJoined;
    }

    private void FixedUpdate()
    {
        if (_gameStart == false) return;
        _waterRb.transform.Translate(Vector3.up * (_waterSpeedMultiplier * Time.fixedDeltaTime));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
        }
    }
}
