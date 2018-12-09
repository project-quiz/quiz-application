using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProtobufEditorWindow : EditorWindow
{
    string[] paths = new string[] { "blabla.proto", "blabla2.proto" };
    string path;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Protobuf")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ProtobufEditorWindow window = (ProtobufEditorWindow)GetWindow(typeof(ProtobufEditorWindow));
        window.titleContent = new GUIContent("Protobuf");
        window.Show();
    }

    protected void OnGUI()
    {
        string[] pathCopy = paths.Clone() as string[];

        for (int i = 0; i < pathCopy.Length; i++)
        {
            paths[i] = EditorGUILayout.TextField($"{i}.", pathCopy[i]);
        }
    }
}