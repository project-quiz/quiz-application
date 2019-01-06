using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class ConnectedScreenUI : MonoBehaviour
{
    [SerializeField] protected TMPro.TextMeshProUGUI text;

    protected void OnEnable()
    {
        ClientService clientService = GlobalServiceLocator.Instance.Get<ClientService>();

        TcpClient tcpClient = clientService.GetClientInformation();

        text.text = $"Connected to: {tcpClient.Client.RemoteEndPoint.ToString()}";
    }
}