using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace MoreFun
{
    public class OpenFolder
    {
        [MenuItem("MoreFun/OpenFolder/dataPath")]
        private static void OpenFolder_dataPath()
        {
            EditorUtility.RevealInFinder(Application.dataPath);
        }
        [MenuItem("MoreFun/OpenFolder/persistentDataPath")]
        private static void OpenFolder_persistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }
        [MenuItem("MoreFun/OpenFolder/streamingAssetsPath")]
        private static void OpenFolder_streamingAssetsPath()
        {
            EditorUtility.RevealInFinder(Application.streamingAssetsPath);
        }
        [MenuItem("MoreFun/OpenFolder/temporaryCachePath")]
        private static void OpenFolder_temporaryCachePath()
        {
            EditorUtility.RevealInFinder(Application.temporaryCachePath);
        }
        [MenuItem("MoreFun/OpenFolder/editor/Log")]
        private static void OpenFolder_editorLog()
        {
            string logPath = Application.persistentDataPath;
            int libIndex;
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                    libIndex = logPath.IndexOf("Library");
                    logPath = logPath.Substring(0, libIndex);
                    Debug.Log(logPath);
                    logPath = logPath + "Library/Logs/Unity/";
                    Debug.Log(logPath);
                    EditorUtility.RevealInFinder(logPath);
                    break;

                case RuntimePlatform.WindowsEditor:
                    libIndex = logPath.IndexOf("AppData");
                    logPath = logPath.Substring(0, libIndex);
                    Debug.Log(logPath);
                    logPath = logPath + "AppData/Local/Unity/Editor/";
                    Debug.Log(logPath);
                    EditorUtility.RevealInFinder(logPath);
                    break;
                default:
                    break;
            }
        }
        [MenuItem("MoreFun/OpenFolder/editor/applicationPath")]
        private static void OpenFolder_editorapplicationPath()
        {
            EditorUtility.RevealInFinder(EditorApplication.applicationPath);
        }
        [MenuItem("MoreFun/OpenFolder/editor/applicationContentsPath")]
        private static void OpenFolder_editorapplicationContentsPath()
        {
            EditorUtility.RevealInFinder(EditorApplication.applicationContentsPath);
        }
    }
    public class EasySaveProject
    {
        private static float ms_lastSaveTime;
        
        [MenuItem("MoreFun/EasySaveProject/Save Project &s")]
        private static void SaveProject()
        {
            EditorApplication.SaveAssets();
            ms_lastSaveTime = Time.realtimeSinceStartup;
            #if LOG_DETAIL
            Debug.Log("Saved Project");
            #endif
        }
        
        [MenuItem("MoreFun/EasySaveProject/Enable Auto Save Project")]
        private static void EnableAutoSaveProject()
        {
            EditorApplication.update += OnEditorUpdate;
            ms_lastSaveTime = Time.realtimeSinceStartup;
            #if LOG_DETAIL
            Debug.Log("Enabled Auto Save Project");
            #endif
        }
        
        [MenuItem("MoreFun/EasySaveProject/Disable Auto Save Project")]
        private static void DisableAutoSaveProject()
        {
            EditorApplication.update -= OnEditorUpdate;
            #if LOG_DETAIL
            Debug.Log("Disabled Auto Save Project");
            #endif
        }
        
        private static void OnEditorUpdate()
        {
            if(Time.realtimeSinceStartup - ms_lastSaveTime > 5.0f)
            {
                EditorApplication.SaveAssets();
            }
        }
    }

    
    public class EasySetTimeScale
    {
        [MenuItem("MoreFun/EasySetTimeScale/timeScale reset to 1.0 &0")]
        private static void EasySetTimeScaleReset()
        {
            Time.timeScale = 1.0f;
            #if LOG_DETAIL
            Debug.Log("timeScale Reseted. timeScale=" + Time.timeScale);
            #endif
        }
        
        [MenuItem("MoreFun/EasySetTimeScale/timeScale set to 0.1")]
        private static void EasySetTimeScaleTo01()
        {
            Time.timeScale = 0.1f;
            #if LOG_DETAIL
            Debug.Log("timeScale set to 0.1f. timeScale=" + Time.timeScale);
            #endif
        }
        
        [MenuItem("MoreFun/EasySetTimeScale/timeScale set to 0.5")]
        private static void EasySetTimeScaleTo05()
        {
            Time.timeScale = 0.5f;
            #if LOG_DETAIL
            Debug.Log("timeScale set to 0.5f. timeScale=" + Time.timeScale);
            #endif
        }
        
        [MenuItem("MoreFun/EasySetTimeScale/timeScale up by 0.1 &=")]
        private static void EasySetTimeScaleUp()
        {
            Time.timeScale = Mathf.Min(3.0f, Time.timeScale + 0.1f);
            #if LOG_DETAIL
            Debug.Log("timeScale up by 0.05f, timeScale=" + Time.timeScale);
            #endif
        }
        
        [MenuItem("MoreFun/EasySetTimeScale/timeScale down by 0.1 &-")]
        private static void EasySetTimeScaleDown()
        {
            Time.timeScale = Mathf.Max(0.0f, Time.timeScale - 0.1f);
            #if LOG_DETAIL
            Debug.Log("timeScale down by 0.05f, timeScale=" + Time.timeScale);
            #endif
        }
		
		[MenuItem("MoreFun/Open new Unity(OSX)")]
		private static void OpenNewUnity()
		{
            OpenNewUnityOSXWindow.Open();
		}

        [MenuItem("Assets/Create/MoreBehavior")]
        private static void CreateMoreBehavior()
        {
            string[] guids = Selection.assetGUIDs;
            string path = string.Empty;
            foreach (string guid in guids) {
                path = AssetDatabase.GUIDToAssetPath (guid);
                break; }
            if (false == Directory.Exists (path))
            {
                path = System.IO.Path.GetDirectoryName (path);
            }
            NewMoreBehaviourWindow.InitPath (path);
        }
    }

}