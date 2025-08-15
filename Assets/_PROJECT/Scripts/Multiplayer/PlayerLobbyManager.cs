using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerLobbyManager : MonoBehaviour
{
    // private PlayerInputManager playerLobby;
    
    [SerializeField] private List<PlayerInput> listOfJoinedPlayers = new List<PlayerInput>();
    [SerializeField] private GameObject playerNameTextPrefab;

    [SerializeField] private VerticalLayoutGroup groupParent;
    
    TMP_Text playerNameText;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // playerLobby = GetComponent<PlayerInputManager>();
        // playerLobby.onPlayerJoined += AddPlayerToLobbyList;
        // playerLobby.onPlayerLeft += RemovePlayerFromLobbyList;
    }

    public void RemovePlayerFromLobbyList(PlayerInput input)
    {
        if (listOfJoinedPlayers.Contains(input))
        {
            listOfJoinedPlayers.Remove(input);
        }
    }

    public void AddPlayerToLobbyList(PlayerInput input)
    {
        Debug.Log("New Player Has Joined");
        listOfJoinedPlayers.Add(input);
        var temporaryNameText = Instantiate(playerNameTextPrefab, groupParent.transform);
        temporaryNameText.GetComponent<TextPlayerData>().ConnectPlayerToThisText(input);
        temporaryNameText.GetComponent<TMP_Text>().text = GenerateNewPlayerName(input.GetComponent<PlayerData>().playerName);
    }

   
    private string GenerateNewPlayerName(string incomingName)
    {
        return $"{incomingName} {listOfJoinedPlayers.Count}";
    }
}
