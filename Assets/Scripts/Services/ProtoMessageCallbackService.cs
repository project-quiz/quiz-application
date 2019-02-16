using Message;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMessageCallbackService : IService
{
    private Dictionary<System.Type, List<object>> actions = new Dictionary<System.Type, List<object>>();

    public void Subscribe<T>(Action<T> action) 
    {
        System.Type t = typeof(T);

        if (!actions.ContainsKey(t))
        {
            actions.Add(t, new List<object>());
        }

        actions[t].Add(action);
    }

    public void SendBaseMessage(BaseMessage baseMessage)
    {
        Get(baseMessage.Message);
    }

    private void SendMessage<T>(T evt)
    {
        System.Type type = typeof(T);

        if (actions.ContainsKey(type))
        {
            foreach (object obj in actions[type])
            {
                (obj as Action<T>)?.Invoke(evt);
            }
        }
    }

    private void Get(Any message)
    {
        switch (message.TypeUrl)
        {
            case ProtoTypeUrl.PlayerJoined:
                SendMessage(PlayerJoined.Parser.ParseFrom(message.Value));
                break;
            case ProtoTypeUrl.GameJoined:
                SendMessage(GameJoined.Parser.ParseFrom(message.Value));
                break;
            default:
                break;
        }
    }
}

public class ProtoTypeUrl
{
    public const string PlayerJoined = "message.PlayerJoined";
    public const string GameJoined = "message.GameJoined";
}