using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TextPlayerData : MonoBehaviour
{
    private PlayerInput connectedPlayer;

    public void ConnectPlayerToThisText(PlayerInput incomingInput)
    {
        connectedPlayer = incomingInput;
    }
    // Update is called once per frame
    public void DestroyThisText()
    {
        Destroy(gameObject);
    }
}
