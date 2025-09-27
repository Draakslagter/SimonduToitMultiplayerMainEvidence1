using TMPro;
using UnityEngine;

public class WinnerUI : MonoBehaviour
{
    [SerializeField] TMP_Text victoryText;
    [SerializeField] TMP_Text timeText;

    private void Start()
    {
        PlayerData.OnPlayerDeath += GetPlayerStats;
    }
    private void GetPlayerStats(Color playerColor, float surviveTime)
    {
        victoryText.color = playerColor;
        timeText.text = $"You Survived " + surviveTime.ToString("F1") + " seconds!";
    }
}
