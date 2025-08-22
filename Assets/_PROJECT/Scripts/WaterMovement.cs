using UnityEngine;

public class WaterMovement : MonoBehaviour
{
    private bool _gameStart;
    private Rigidbody _waterRb;
    private float _waterSpeedMultiplier;
    
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
        Debug.Log("Moving Water");
        _waterRb.transform.Translate(Vector3.up * (_waterSpeedMultiplier * Time.fixedDeltaTime));
    }
}
