using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class BlockSpawner : MonoBehaviour
{
    private static BlockSpawner _instance;
    private static readonly int ObjectColor = Shader.PropertyToID("_objectColor");
    public static BlockSpawner Instance => _instance;
    
    [SerializeField] private List<GameObject> blockPrefabs;
    [SerializeField] private float sizeRange;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    
    public void SpawnBlock(PlayerMovementAndControlSetup player)
    {
        
        var temporaryObject = Instantiate(blockPrefabs[Random.Range(0, blockPrefabs.Count)], player.pickUpTransform.position, player.pickUpTransform.rotation, player.pickUpTransform);
        var randomColour = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        temporaryObject.GetComponent<Renderer>().material.SetColor(ObjectColor, randomColour);
        
        player.interactionTransform = temporaryObject.transform;
        player.interactionRb = temporaryObject.GetComponent<Rigidbody>();
        var randomNumber = Random.Range(0.9f, sizeRange);
        var randomScale = temporaryObject.transform.localScale * randomNumber;
        temporaryObject.transform.DOScale(randomScale, 0.5f).SetEase(Ease.InBounce);
    }
}
