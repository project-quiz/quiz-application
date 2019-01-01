using Data;
using Google.Protobuf;
using System;
using System.Collections;
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

    private object listenContext = new object();

    public ClientService(ServerSettingsLibrary serverSettingsLibrary, MainThreadService mainThreadService, ProtoMessageCallbackService protoMessageCallbackService)
    {
        this.serverSettingsLibrary = serverSettingsLibrary;
        this.mainThreadService = mainThreadService;
        this.protoMessageCallbackService = protoMessageCallbackService;
    }
    
    public void ConnectAndListen()
    {
        IPAddress localAddr = IPAddress.Parse(serverSettingsLibrary.IpAddress);
        client = new TcpClient();

        try
        {
            //Change to the ip adress of the server.
            client.Connect(localAddr, serverSettingsLibrary.Port);
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
                //Connected :)
                ConnectedEvent?.Invoke();
            }
        }

        if (client.Connected)
        {
            networkStream = client.GetStream();
            Write("HELLO THIS IS A CLIENT");

            //Start listening
            Debug.Log("Start listen thread");
            Thread thread = new Thread(Listen);
            thread.Start();
        }
    }

    private void Listen()
    {
        while (true)
        {
            try
            {
                Byte[] receivedBytes = new byte[1024];



                Debug.Log("Reading from stream");
                int receivedAmount = networkStream.Read(receivedBytes, 0, receivedBytes.Length);
                Debug.Log("done reading ");

                if (receivedBytes.Length > 0)
                {
                    //amount received fits in the byte array sent message
                    if (receivedAmount <= receivedBytes.Length)
                    {
                        mainThreadService.SendToMainThread(() =>
                        {
                            byte[] trimmedArray = new byte[receivedAmount];
                            for (int i = 0; i < receivedAmount; i++)
                            {
                                trimmedArray[i] = receivedBytes[i];
                            }

                            BaseMessage baseMessage = BaseMessage.Parser.ParseFrom(trimmedArray);
                            protoMessageCallbackService.SendBaseMessage(baseMessage);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Exception : " + ex.ToString());
            }
            finally
            {
                if (!client.Connected)
                {
                    Debug.Log("Connection lost :(");
                    mainThreadService.SendToMainThread(() =>
                    {
                        DisconnectedEvent?.Invoke();
                    });
                    //TODO add reconnection to the server.
                }
            }
        }
    }
    
    public void Write(string message)
    {
        if (client.Connected)
        {
            //Always end in '\n' because that is the server end char.
            Byte[] data = Encoding.ASCII.GetBytes("HELLO FROM CLIENT" + "\n");
            networkStream.Write(data, 0, data.Length);
        }
    }

    public void Dispose()
    {
        client.Close();
    }
}