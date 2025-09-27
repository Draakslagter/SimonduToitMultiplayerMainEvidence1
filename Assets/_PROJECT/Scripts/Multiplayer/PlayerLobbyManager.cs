using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerLobbyManager : MonoBehaviour
{
    private static PlayerLobbyManager _instance;
    public static PlayerLobbyManager Instance => _instance;
    
    [SerializeField] private List<PlayerInput> listOfJoinedPlayers = new List<PlayerInput>();
    [SerializeField] private GameObject playerNameTextPrefab;

    // [SerializeField] private VerticalLayoutGroup groupParent;
    
    // TMP_Text playerNameText;

    [SerializeField] private CinemachineTargetGroup cineMachineTargetGroup;
    
    [SerializeField] private CanvasGroup startGameCanvasGroup;
    [SerializeField] private CanvasGroup endGameCanvasGroup;
    
    
    public UnityEvent<bool> triggerGameStart;
    public UnityEvent<CanvasGroup, bool> triggerMenuChange;
    public UnityEvent triggerPlayerJoined;

    private PlayerInputManager _playerInputManager;
    
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
        
        if (_playerInputManager == null)
        {
            _playerInputManager = GetComponent<PlayerInputManager>();
        }
    }
    public void AddPlayerToLobbyList(PlayerInput input)
    {
        listOfJoinedPlayers.Add(input);
        cineMachineTargetGroup.AddMember(input.gameObject.transform, 1, 1);
        // var temporaryNameText = Instantiate(playerNameTextPrefab, groupParent.transform);
        // temporaryNameText.GetComponent<TextPlayerData>().ConnectPlayerToThisText(input);
        // temporaryNameText.GetComponent<TMP_Text>().text = GenerateNewPlayerName(input.GetComponent<PlayerData>().playerName);
        CheckLobbySize();
    }
    
    public void RemovePlayerFromLobbyList(PlayerInput input)
    {
        if (listOfJoinedPlayers.Contains(input))
        {
            listOfJoinedPlayers.Remove(input);
        }
        CheckLobbySize();
    }

    private void CheckLobbySize()
    {
        if (listOfJoinedPlayers.Count <= 0)
        {
            _playerInputManager.enabled = false;
            triggerMenuChange.Invoke(endGameCanvasGroup, true);
        }
        else
        {
            if (listOfJoinedPlayers.Count <= 1)
            {
                triggerPlayerJoined.Invoke();
                return;
            }
            triggerMenuChange.Invoke(startGameCanvasGroup, false);
        }
        StartCoroutine(CheckStartGame());
    }

    private IEnumerator CheckStartGame()
    {
        yield return new WaitForEndOfFrame();
        triggerGameStart.Invoke(listOfJoinedPlayers.Count >= 2);
    }
    // private string GenerateNewPlayerName(string incomingName)
    // {
    //     return $"{incomingName} {listOfJoinedPlayers.Count}";
    // }
}
