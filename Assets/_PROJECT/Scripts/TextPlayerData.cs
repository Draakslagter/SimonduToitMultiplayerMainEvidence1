using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TextPlayerData : MonoBehaviour
{
    private PlayerInput connectedPlayer;
    private PlayerMovementAndControlSetup playerScript;

    private void Start()
    {
        playerScript = connectedPlayer.gameObject.GetComponent<PlayerMovementAndControlSetup>();
        playerScript.triggerDestroy.AddListener(DestroyThisText);
    }

    public void ConnectPlayerToThisText(PlayerInput incomingInput)
    {
        connectedPlayer = incomingInput;
    }
    // Update is called once per frame
    private void DestroyThisText()
    {
        Destroy(gameObject);
    }
}
