using Google.Protobuf;
using Message;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class ProtoTypeService : IService
{
    public Dictionary<string, Type> typeUriToType = new Dictionary<string, Type>();

    public ProtoTypeService()
    {
        IEnumerable<Type> types = TypeCache.GetTypesAssignableTo<IMessage>();

        foreach (var item in types)
        {
            if (item.GetTypeInfo().IsInterface || item.GetTypeInfo().IsAbstract || item.GetTypeInfo().IsGenericType)
            {
                continue;
            }
            typeUriToType.Add(item.FullName.ToLower(), item);
        }
    }

    public IMessage GetInstanceOfTypeUrl(string typeUrl)
    {
        Type returnType = null;

        typeUriToType.TryGetValue(typeUrl.ToLower(), out returnType);

        if (returnType != null)
        {
            return Activator.CreateInstance(returnType) as IMessage;
        }

        return null;
    }
}