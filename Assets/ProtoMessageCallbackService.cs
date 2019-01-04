using Data;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ProtoMessageCallbackService : IService
{
    private Dictionary<Type, List<object>> actions = new Dictionary<Type, List<object>>();

    public void Subscribe<T>(Action<T> action) 
    {
        Type t = typeof(T);

        if (!actions.ContainsKey(t))
        {
            actions.Add(t, new List<object>());
        }

        actions[t].Add(action);
    }

    public void SendBaseMessage(BaseMessage baseMessage)
    {
        //Get(baseMessage.Type, baseMessage.Message);
    }

    private void SendMessage<T>(T evt)
    {
        Type type = typeof(T);

        if (actions.ContainsKey(type))
        {
            foreach (object obj in actions[type])
            {
                (obj as Action<T>)?.Invoke(evt);
            }
        }
    }

    private void Get(int index, Google.Protobuf.WellKnownTypes.Any anyMessage)
    {
        switch (index)
        {
            case 0:
                SendMessage(Player.Parser.ParseFrom(anyMessage.Value));
                return;
            case 1:
                SendMessage(PlayerJoined.Parser.ParseFrom(anyMessage.Value));
                return;
        }

        Debug.LogError($"Unknown any type of {anyMessage.TypeUrl}");
    }
}

public static class ProtoTypeConverter
{
    public static Dictionary<int, Type> IntToType = new Dictionary<int, Type>
    {
        {1, typeof(PlayerJoined) }
    };

    public static Dictionary<string, Type> StringToType = new Dictionary<string, Type>
    {
        { "Data.PlayerJoined", typeof(PlayerJoined) }
    };

    public static Dictionary<Type, string> TypeToString = new Dictionary<Type, string>
    {
        { typeof(PlayerJoined), "Data.PlayerJoined" }
    };
}
