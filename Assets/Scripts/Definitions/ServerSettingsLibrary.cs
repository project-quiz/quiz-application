using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(ServerSettingsLibrary), menuName = "Game/ServerSettingsLibrary")]
public class ServerSettingsLibrary : ScriptableObject, IService
{
    [SerializeField] protected EnviormentType currentEnviormentType;
    [SerializeField] protected ServerData[] serverList;

    public enum EnviormentType
    {
        LOCAL = 0,
        NAS = 1
    }

    [Serializable]
    public class ServerData
    {
        public EnviormentType ServerType { get { return serverType; } }
        [SerializeField] private EnviormentType serverType;

        public string IpAddress { get { return ipAddress; } }
        [SerializeField] private string ipAddress;

        public int Port { get { return port; } }
        [SerializeField] private int port;
    }

    public ServerData GetCurrentServerData()
    {
        return serverList.FirstOrDefault(x => x.ServerType == currentEnviormentType);
    }
}