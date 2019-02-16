using Message;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class ClientService : IService, IDisposable
{
    public delegate void ConnectedHandler();
    public event ConnectedHandler ConnectedEvent;
    public event ConnectedHandler DisconnectedEvent;

    public delegate void MessageReceivedHandler(IMessage message);
    public event MessageReceivedHandler MessageReceivedEvent;

    private TcpClient client;
    private NetworkStream networkStream;
    private int retries;
    private const int maxRetries = 3;

    private ServerSettingsLibrary serverSettingsLibrary;
    private MainThreadService mainThreadService;
    private ProtoMessageCallbackService protoMessageCallbackService;

    private bool disposed;

    public ClientService(ServerSettingsLibrary serverSettingsLibrary, MainThreadService mainThreadService, ProtoMessageCallbackService protoMessageCallbackService)
    {
        this.serverSettingsLibrary = serverSettingsLibrary;
        this.mainThreadService = mainThreadService;
        this.protoMessageCallbackService = protoMessageCallbackService;
    }

    public TcpClient GetClientInformation()
    {
        return client;
    }
    
    public void ConnectAndListen()
    {
        ServerSettingsLibrary.ServerData serverData = serverSettingsLibrary.GetCurrentServerData();

        IPAddress localAddr = IPAddress.Parse(serverData.IpAddress);
        client = new TcpClient();

        try
        {
            //Change to the ip adress of the server.
            client.Connect(localAddr, serverData.Port);
            Debug.Log("Connected to: " + client.Connected);
        }
        catch (SocketException ex)
        {
            Debug.LogError(ex.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
        finally
        {
            if (!client.Connected && retries < maxRetries)
            {
                retries++;
                ConnectAndListen();
            }
            else if(retries == maxRetries)
            {
                //Not connected cancel.
                DisconnectedEvent?.Invoke();
            }
            else
            {
                //Connected waiting for auth :)
                ConnectedEvent();
            }
        }

        if (client.Connected)
        {
            networkStream = client.GetStream();

            SentJoinMessage();

            //Start listening
            Debug.Log("Start listen thread");
            Task.Run(ListenAsync);
        }
    }

    private void SentJoinMessage()
    {
        PlayerJoin playerJoin = new PlayerJoin();
        playerJoin.Nickname = GlobalServiceLocator.Instance.Get<PlayerService>().Nickname;

        WriteAsync(playerJoin);
    }

    private async void ListenAsync()
    {
        while (!disposed)
        {
            byte[] bytes = await GetMessageAsync();

            if (bytes != null)
            {
                mainThreadService.SendToMainThread(() =>
                {
                    BaseMessage baseMessage = BaseMessage.Parser.ParseFrom(bytes);
                    protoMessageCallbackService.SendBaseMessage(baseMessage);
                });
            }
        }
    }

    private async Task<byte[]> GetMessageAsync()
    {
        byte[] result = null;
        try
        {
            Byte[] receivedBytes = new byte[1024];

            Debug.Log("Reading from stream");
            int receivedAmount = await networkStream.ReadAsync(receivedBytes, 0, receivedBytes.Length);
            result = TrimBytes(receivedBytes, receivedAmount);
            Debug.Log($"Received data {receivedAmount}");
        }
        catch (ObjectDisposedException ex)
        {
            Debug.LogWarning("Connection lost with the server with ex:" + ex.ToString());
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception : " + ex.ToString());
        }
        finally
        {
            if (!client.Connected)
            {
                disposed = true;
                Debug.Log("Connection lost :(");
                mainThreadService.SendToMainThread(() =>
                {
                    DisconnectedEvent?.Invoke();
                });
            }
        }
        return result;
    }

    private byte[] TrimBytes(byte[] bytes, int receivedAmount)
    {
        //if bytes is same size as receieved skip copying.
        if (bytes.Length == receivedAmount)
        {
            return bytes;
        }

        byte[] trimmedArray = new byte[receivedAmount];
        for (int i = 0; i < receivedAmount; i++)
        {
            trimmedArray[i] = bytes[i];
        }
        return trimmedArray;
    }

    public void WriteAsync(IMessage message)
    {
        Any any = Any.Pack(message);

        BaseMessage baseMessage = new BaseMessage
        {
            Id = 1,
            Message = any,
        };

        WriteAsync(baseMessage.ToByteArray());
    }
    
    public async void WriteAsync(byte[] bytes)
    {
        List<byte> modifiedBytes = new List<byte>();

        for (int i = 0; i < bytes.Length; i++)
        {
            modifiedBytes.Add(bytes[i]);
        }

        modifiedBytes.AddRange(new byte[] { (byte)'[', (byte)'E', (byte)'N', (byte)'D', (byte)']'});

        if (client.Connected)
        {
            Debug.Log("Sent :" + modifiedBytes.Count);

            await networkStream.WriteAsync(modifiedBytes.ToArray(), 0, modifiedBytes.Count);
        }
    }

    public void Dispose()
    {
        Debug.Log("Dispose");
        disposed = true;
        if (client != null)
        {
            client.Close();
        }
    }
}