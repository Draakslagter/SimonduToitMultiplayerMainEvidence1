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
        PlayerLobbyManager.Instance.triggerGameStart.AddListener(StartMovingWater);
    }

    private void StartMovingWater(bool playerJoined)
    {
        _gameStart = playerJoined;
    }

    private void FixedUpdate()
    {
        if (_gameStart == false) return;
        _waterRb.transform.Translate(Vector3.up * (_waterSpeedMultiplier * Time.fixedDeltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDrownable>() == null) return;
        var drownable = other.gameObject.GetComponent<IDrownable>();
        drownable.Drown(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<IDrownable>() == null) return;
        var drownable = other.gameObject.GetComponent<IDrownable>();
        drownable.Drown(false);
    }
}
