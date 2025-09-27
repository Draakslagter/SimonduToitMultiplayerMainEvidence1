using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PlayerData : MonoBehaviour, IDrownable
{
    private static readonly int Color1 = Shader.PropertyToID("_BaseColor");

    [Header ("Visuals")]
    [SerializeField]  private Mesh[] fishMeshes;
    private MeshFilter meshFilter;
    private Renderer _playerRenderer;
    private Color _playerColor;
    
    
    [Header ("Stats")]
    [SerializeField] private float playerHealth = 5f;
    private float _surviveTime;
    private bool _drowning;

    private PlayerInput _playerInputData;
    
    public static Action<Color, float> OnPlayerDeath;

    private void Awake()
    {
        if (_playerRenderer == null)
        {
            _playerRenderer = GetComponent<Renderer>();
        }

        if (meshFilter == null)
        {
            meshFilter = GetComponent<MeshFilter>();
        }
    }

    private void Start()
    {
        _playerColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        _playerRenderer.material.SetColor(Color1, _playerColor);
        meshFilter.mesh = fishMeshes[Random.Range(0, fishMeshes.Length)];
    }
    private void Update()
    {
        _surviveTime += Time.deltaTime;
        Debug.Log(_surviveTime);
        if (!_drowning) return;
        playerHealth -= Time.deltaTime;
        if (!(playerHealth <= 0f)) return;
        SendData();
        Destroy(gameObject);
    }

    public void SendData()
    {
        OnPlayerDeath.Invoke(_playerColor, _surviveTime);
    }
    public void Drown(bool  submerged)
    {
        _drowning = submerged;
    }
}
