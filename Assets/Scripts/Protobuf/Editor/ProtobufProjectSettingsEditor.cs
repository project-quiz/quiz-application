using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityLog = UnityEngine.Debug;

[CustomEditor(typeof(ProtobufProjectSettings))]
public class ProtobufProjectSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ProtobufProjectSettings settings = target as ProtobufProjectSettings;

        base.OnInspectorGUI();

        if(GUILayout.Button("Compile Protobuf Files"))
        {
            CompileProtobufFiles(settings);
        }
    }

    private void CompileProtobufFiles(ProtobufProjectSettings settings)
    {
        UnityLog.Log("Start Compiling");

        foreach (var fileInfo in settings.PathInfos)
        {
            string path = Application.dataPath.Replace("/Assets","/");
            string fullPath = path + fileInfo.Path;

            if (fileInfo.IsFile)
            {
                CompileFile(settings, fullPath);
            }
            else
            {
                string[] filePaths = Directory.GetFiles(fullPath, "*.proto", SearchOption.AllDirectories);
                foreach (var item in filePaths)
                {
                    CompileFile(settings, item);
                }
            }
        }

        UnityLog.Log("End Compiling");
    }

    private void CompileFile(ProtobufProjectSettings settings, string path)
    {
        UnityLog.Log($"Next Item : {path}");

        string arguments = $@"--csharp_out={settings.OutputPath} --proto_path=C:\Users\guido\.nuget\packages\google.protobuf.tools\3.6.1\tools {path}";
        Process process = new Process();
        process.StartInfo.FileName = ProtobufPrerenceItemSettings.ProtocExecutablePath;
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.RedirectStandardOutput = true;
        process.OutputDataReceived += OnOutputDataReceived;

        process.Start();

        process.WaitForExit();
        UnityLog.Log(process.ExitCode);
    }

    private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
    {
        UnityLog.Log($"DATA:{e.Data}");
    }
}