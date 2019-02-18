using Message;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using Google.Protobuf.Reflection;
using Google.Protobuf;
using System.Reflection;

public class ProtoMessageCallbackService : IService
{
    private Dictionary<System.Type, List<object>> actions = new Dictionary<System.Type, List<object>>();

    private ProtoTypeService protoTypeService;

    public ProtoMessageCallbackService(ProtoTypeService protoTypeService)
    {
        this.protoTypeService = protoTypeService;
    }

    public void Subscribe<T>(Action<T> action) 
    {
        System.Type t = typeof(T);

        if (!actions.ContainsKey(t))
        {
            actions.Add(t, new List<object>());
        }

        actions[t].Add(action);
    }

    public void ReceiveBaseMessage(BaseMessage baseMessage)
    {
        Get(baseMessage.Message);
    }

    private void Get(Any message)
    {
        IMessage instance = protoTypeService.GetInstanceOfTypeUrl(message.TypeUrl);
        instance.MergeFrom(message.Value);

        MethodInfo method = typeof(ProtoMessageCallbackService).GetMethod("SendMessage");
        method = method.MakeGenericMethod(instance.GetType());
        method.Invoke(this, new object[] { instance });
    }

    public void SendMessage<T>(T evt)
    {
        System.Type type = evt.GetType();

        UnityEngine.Debug.Log(type);
        UnityEngine.Debug.Log(evt);

        if (actions.ContainsKey(type))
        {
            foreach (object obj in actions[type])
            {
                (obj as Action<T>)?.Invoke(evt);
            }
        }
    }
}