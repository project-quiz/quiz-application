using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ServerSettingsLibrary), menuName = "Game/ServerSettingsLibrary")]
public class ServerSettingsLibrary : ScriptableObject, IService
{
    public string IpAddress { get { return ipAddress; } }
    [SerializeField] private string ipAddress;

    public int Port { get { return port; } }
    [SerializeField] private int port;
}