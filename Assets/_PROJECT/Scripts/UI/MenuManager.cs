using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private static MenuManager _instance;
    public static MenuManager Instance => _instance;


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

    private void Start()
    {
        PlayerLobbyManager.Instance.triggerMenuChange.AddListener(ActivateMenu);
    }

    private void ActivateMenu(CanvasGroup canvasGroup, bool isActive)
    {
        if (canvasGroup == null) return;
        canvasGroup.alpha = isActive ? 1 : 0;
        canvasGroup.blocksRaycasts = isActive;
        canvasGroup.interactable = isActive;
        canvasGroup.gameObject.SetActive(isActive);
    }
}
