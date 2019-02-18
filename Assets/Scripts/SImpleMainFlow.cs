using Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temp main flow because of error in the state machines :(
/// </summary>
public partial class SImpleMainFlow : MonoBehaviour
{
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject connectingScreen;
    [SerializeField] private GameObject connectedScreen;
    [SerializeField] private GameObject disconnectedScreen;
    [SerializeField] private GameObject questionScreen;
    [SerializeField] private GameObject joinedScreen;
    [SerializeField] private GameObject gameJoinedScreen;

    private ClientService clientService;
    private ProtoMessageCallbackService protoMessageCallbackService;

    private ScreenStates currentScreenState;

    public void SwitchScreen(ScreenStates state)
    {
        currentScreenState = state;

        startScreen.SetActive(currentScreenState == ScreenStates.Start);
        connectingScreen.SetActive(currentScreenState == ScreenStates.Connecting);
        connectedScreen.SetActive(currentScreenState == ScreenStates.Connected);
        disconnectedScreen.SetActive(currentScreenState == ScreenStates.Disconnected);
        questionScreen.SetActive(currentScreenState == ScreenStates.Question);
        joinedScreen.SetActive(currentScreenState == ScreenStates.Joined);
        gameJoinedScreen.SetActive(currentScreenState == ScreenStates.GameJoin);
    }

    protected void Awake()
    {
        SwitchScreen(ScreenStates.Connecting);

        protoMessageCallbackService = GlobalServiceLocator.Instance.Get<ProtoMessageCallbackService>();
        protoMessageCallbackService.Subscribe<ServerJoined>(OnServerJoined);
        protoMessageCallbackService.Subscribe<GameJoined>(OnGameJoined);

        clientService = GlobalServiceLocator.Instance.Get<ClientService>();
        clientService.ConnectedEvent += OnClientConnected;
        clientService.DisconnectedEvent += OnDisconnectedEvent;
    }

    protected void OnDestroy()
    {
        clientService.ConnectedEvent -= OnClientConnected;
        clientService.DisconnectedEvent -= OnDisconnectedEvent;
        clientService.Dispose();
    }

    private void OnServerJoined(ServerJoined serverJoined)
    {
        Debug.Log("OnPlayerJoined 1");

        SwitchScreen(ScreenStates.Joined);
    }

    private void OnGameJoined(GameJoined gameJoined)
    {
        SwitchScreen(ScreenStates.GameJoin);
    }

    protected void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20));
        if (currentScreenState == ScreenStates.Disconnected)
        {
            GUILayout.Button("Retry");
        }
        GUILayout.EndArea();
    }

    private void OnDisconnectedEvent()
    {
        SwitchScreen(ScreenStates.Disconnected);
    }

    private void OnClientConnected()
    {
        SwitchScreen(ScreenStates.Connected);
    }
}