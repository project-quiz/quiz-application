using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProtobufPrerenceItemSettings
{
    public static string ProtocExecutablePath { get { return EditorPrefs.GetString(EDITOR_PREF_PROTOC_EXECUTABLE_PATH); } }

    private const string EDITOR_PREF_PROTOC_EXECUTABLE_PATH = "EDITOR_PREF_PROTOC_EXECUTABLE_PATH";

    // Add preferences section named "My Preferences" to the Preferences Window
    [PreferenceItem("Protobuf Settings")]
    public static void PreferencesGUI()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.TextField("Protoc Executable Path", ProtocExecutablePath);
        if(GUILayout.Button("Select Path", GUILayout.Width(150)))
        {
            SelectPath();
        }
        GUILayout.EndHorizontal();
    }

    private static void SelectPath()
    {
        string path = EditorUtility.OpenFilePanel("Select Protoc Executable", "", "exe");
        EditorPrefs.SetString(EDITOR_PREF_PROTOC_EXECUTABLE_PATH, path);
    }
}
