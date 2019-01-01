using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temp main flow because of error in the state machines :(
/// </summary>
public class SImpleMainFlow : MonoBehaviour
{
    [SerializeField] private GameObject connectingScreen;
    [SerializeField] private GameObject connectedScreen;
    [SerializeField] private GameObject disconnectedScreen;

    private ClientService clientService;
    private ProtoMessageCallbackService protoMessageCallbackService;

    private States currentState;

    private enum States
    {
        Connecting,
        Connected,
        Disconnected
    }

    protected void Awake()
    {
        SwitchScreen(States.Connecting);

        protoMessageCallbackService = GlobalServiceLocator.Instance.Get<ProtoMessageCallbackService>();
        protoMessageCallbackService.Subscribe<PlayerJoined>(OnPlayerJoined);

        clientService = GlobalServiceLocator.Instance.Get<ClientService>();
        clientService.ConnectedEvent += OnClientConnected;
        clientService.DisconnectedEvent += OnDisconnectedEvent;
    
        clientService.ConnectAndListen();
    }

    protected void OnDestroy()
    {
        clientService.ConnectedEvent -= OnClientConnected;
        clientService.DisconnectedEvent -= OnDisconnectedEvent;
    }

    private void OnPlayerJoined(PlayerJoined playerJoined)
    {
        Debug.Log(playerJoined.Player.Guid);
    }

    protected void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
        if (currentState == States.Disconnected)
        {
            GUILayout.Button("Retry");
        }
        GUILayout.EndArea();
    }

    private void OnDisconnectedEvent()
    {
        SwitchScreen(States.Disconnected);
    }

    private void OnClientConnected()
    {
        SwitchScreen(States.Connected);
    }

    private void SwitchScreen(States state)
    {
        currentState = state;

        connectingScreen.SetActive(currentState == States.Connecting);
        connectedScreen.SetActive(currentState == States.Connected);
        disconnectedScreen.SetActive(currentState == States.Disconnected);
    }
}