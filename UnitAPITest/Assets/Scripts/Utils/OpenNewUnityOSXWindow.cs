using System;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace MoreFun
{
    public class OpenNewUnityOSXWindow : EditorWindow
    {
        private const string PrefKey_UnityPath = "OpenNewUnityOSXWindow_UnityPath";

        public static void Open()
        {
            OpenNewUnityOSXWindow window =
                (OpenNewUnityOSXWindow)EditorWindow.GetWindow
                (typeof(OpenNewUnityOSXWindow));
            window.title = "Open new Unity (OSX)";
            window.minSize = new Vector2 (100, 40);
        }


        void OnGUI()
        {
            string unityPath = EditorPrefs.GetString(PrefKey_UnityPath);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("source path: ");
            unityPath = EditorGUILayout.TextField(unityPath);
            EditorPrefs.SetString(PrefKey_UnityPath, unityPath);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            if(GUILayout.Button("OK"))
            {
                OpenNewUnity(unityPath);
                Close();
            }

        }

        private static void OpenNewUnity(string unityPath)
        {
            string toolDirPath = Application.dataPath;

            UnityEngine.Debug.Log("open -n " + unityPath);

            if(File.Exists(unityPath) || Directory.Exists(unityPath))
            {
                try
                {

                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.WorkingDirectory = toolDirPath;
                    proc.StartInfo.FileName = "open";
                    proc.StartInfo.Arguments = "-n " + unityPath;
                    proc.StartInfo.RedirectStandardOutput = false;
                    proc.Start();

                    //proc.WaitForExit();
                    //int exitCode = proc.ExitCode;

                    //UnityEngine.Debug.Log("result:" + proc.StandardOutput.ReadToEnd());
                } catch (System.Exception ex)
                {
                    UnityEngine.Debug.LogError("error=" + ex);
                }
            }
            else
            {
                Debug.LogError(unityPath + " does NOT exist.");
            }
        }
    }
}

