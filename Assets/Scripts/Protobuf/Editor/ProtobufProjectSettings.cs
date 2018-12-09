using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProtobufProjectSettings", menuName = "Data/Protobuf Project Settings")]
public class ProtobufProjectSettings : ScriptableObject
{
    [Serializable]
    public struct PathInfo
    {
        [SerializeField] public string Path;
        [SerializeField] public bool IsFile;
    }

    [SerializeField] public string OutputPath;
    [SerializeField] public PathInfo[] PathInfos;
}
