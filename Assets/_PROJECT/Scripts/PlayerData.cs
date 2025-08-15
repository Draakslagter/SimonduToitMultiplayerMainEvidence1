using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerData : MonoBehaviour
{
    public string playerName;
    [SerializeField] private float playerHealth;
    [SerializeField] private float playerScore;

    private PlayerInput _playerInputData;

    private void Awake()
    {
        _playerInputData = GetComponent<PlayerInput>();
    }
}
