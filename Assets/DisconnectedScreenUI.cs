using UnityEngine;
using UnityEngine.UI;

public class DisconnectedScreenUI : MonoBehaviour
{
    [SerializeField] private Button retryButton;

    protected void Awake()
    {
        retryButton.onClick.AddListener(OnRetryButtonClicked);
    }

    private void OnRetryButtonClicked()
    {
        SImpleMainFlow simpleMainFlow = FindObjectOfType<SImpleMainFlow>();
        simpleMainFlow.SwitchScreen(ScreenStates.Start);
    }
}