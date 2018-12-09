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
        DrawExecutablePath("Protoc Executable Path", "", "exe", EDITOR_PREF_PROTOC_EXECUTABLE_PATH, true);
    }

    private static void DrawExecutablePath(string title, string folder, string extention, string prefKey, bool isFile)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.TextField(title, EditorPrefs.GetString(prefKey));
        if (GUILayout.Button("Select Path", GUILayout.Width(150)))
        {
            SelectPath(title, folder, extention, prefKey, isFile);
        }
        GUILayout.EndHorizontal();
    }

    private static void SelectPath(string title, string folder, string extention, string prefKey, bool isFile)
    {
        string path = isFile ? EditorUtility.OpenFilePanel(title, folder, extention) : EditorUtility.OpenFolderPanel(title, folder, "");
        EditorPrefs.SetString(prefKey, path);
    }
}
