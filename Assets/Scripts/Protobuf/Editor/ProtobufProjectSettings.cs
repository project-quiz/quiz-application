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
        public string Path { get { return path; } }
        [SerializeField] private string path;
        public bool IsFile { get { return isFile; } }
        [SerializeField] private bool isFile;
    }

    [SerializeField] private PathInfo[] pathInfos;
}
