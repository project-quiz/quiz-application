using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService : IService
{
    public Player LocalPlayer { get; private set; }
    public string Nickname = "BlackSpider";

    public PlayerService(ProtoMessageCallbackService protoMessages)
    {
        protoMessages.Subscribe<PlayerJoined>(OnPlayerJoined);
    }

    private void OnPlayerJoined(PlayerJoined playerJoined)
    {
        Debug.Log("OnPlayerJoined 2");
        LocalPlayer = playerJoined.Player;
    }
}
