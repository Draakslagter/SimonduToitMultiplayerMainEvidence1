using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private VerticalLayoutGroup groupParent;
    
    TMP_Text playerNameText;

    public UnityEvent<bool> triggerWaterMovement;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    public void AddPlayerToLobbyList(PlayerInput input)
    {
        Debug.Log("New Player Has Joined");
        listOfJoinedPlayers.Add(input);
        var temporaryNameText = Instantiate(playerNameTextPrefab, groupParent.transform);
        temporaryNameText.GetComponent<TextPlayerData>().ConnectPlayerToThisText(input);
        temporaryNameText.GetComponent<TMP_Text>().text = GenerateNewPlayerName(input.GetComponent<PlayerData>().playerName);
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
        triggerWaterMovement.Invoke(listOfJoinedPlayers.Count > 0);
    }
    private string GenerateNewPlayerName(string incomingName)
    {
        return $"{incomingName} {listOfJoinedPlayers.Count}";
    }
}
