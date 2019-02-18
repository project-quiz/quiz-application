using Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerService : IService
{
    public Dictionary<string, Player> Players = new Dictionary<string, Player>();
    public Player LocalPlayer { get; private set; }
    public string DefaultNickName = "Player";

    private ClientService clientService;

    public PlayerService(ProtoMessageCallbackService protoMessages)
    {
        protoMessages.Subscribe<ServerJoined>(OnServerJoined);
        protoMessages.Subscribe<PlayersNickNameChanged>(OnNickNameChanged);
    }

    public void UpdateNickName(string name)
    {
        if(LocalPlayer != null)
        {
            LocalPlayer.Nickname = name;
        }
    }

    private void OnNickNameChanged(PlayersNickNameChanged playersNickNameChanged)
    {
        if (IsLocalPlayer(playersNickNameChanged.Guid))
        {

        }
    }

    private void OnServerJoined(ServerJoined serverJoined)
    {
        LocalPlayer = serverJoined.Player;
    }

    private bool IsLocalPlayer(Player player)
    {
        return IsLocalPlayer(player?.Guid);
    }

    private bool IsLocalPlayer(string guid)
    {
        return LocalPlayer != null ? LocalPlayer?.Guid == guid : false;
    }
}
