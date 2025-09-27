using TMPro;
using UnityEngine;

public class PlayerCount : MonoBehaviour
{
    [SerializeField] private int playersToStart = 2;
    [SerializeField] private TMP_Text playerCountText;

    private void Start()
    {
        PlayerLobbyManager.Instance.triggerPlayerJoined.AddListener(PlayerCountDown);
    }

    private void PlayerCountDown()
    {
        playersToStart--;
        playerCountText.text = $"{playersToStart}";
    }
}
