using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

namespace MoreFun
{
    public class NewMoreBehaviourWindow : EditorWindow
    {
        private string path;
        private bool _didFocus = false;
        private string _nameSpaceName;
        private string _newBehaviourName;

        public static void InitPath(string path)
        {
            NewMoreBehaviourWindow window =
                (NewMoreBehaviourWindow)EditorWindow.GetWindow
                    (typeof(NewMoreBehaviourWindow));
            window.Init();
            window.path = path;
            window.title = "NewMoreBehavior";
            window.minSize = new Vector2 (100, 40);
        }

        private void Init()
        {
            _didFocus = false;
            _nameSpaceName = "NewBorn";
            _newBehaviourName = "NewMoreBehavior";
        }

        void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("namespace");
            _nameSpaceName = EditorGUILayout.TextField(_nameSpaceName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("class");
            GUI.SetNextControlName("NewMoreBehavior");
            _newBehaviourName = EditorGUILayout.TextField(_newBehaviourName);
            if(false == _didFocus)
            {
                EditorGUI.FocusTextInControl("NewMoreBehavior");
                _didFocus = true;
            }

            EditorGUILayout.EndHorizontal();

            bool returnHit = false;
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.keyDown:
                {
                    if (Event.current.keyCode == (KeyCode.Return))
                    {
                        returnHit = true;
                    }
                    break;
                }
            }

            if ((returnHit || GUILayout.Button("OK")) &&
                !string.IsNullOrEmpty(_nameSpaceName) &&
                !string.IsNullOrEmpty(_newBehaviourName))
            {
                _CreateExtendBehavior(path, _nameSpaceName, _newBehaviourName);
                Close ();
            }
        }
        private static void _CreateExtendBehavior(string path, string nameSpaceName, string behaviourName)
        {
            string content =
                System.IO.File.ReadAllText(@Application.dataPath +
                                           "/Standard Assets/MoreFun/Core/MoreBehaviourTemplate.cs");
            content = content.Replace("NamespaceTemplate",
                                      nameSpaceName);
            content = content.Replace("MoreBehaviourTemplate",
                                      behaviourName);
            File.WriteAllText (path + "/" + behaviourName+ ".cs",
                               content);
            AssetDatabase.Refresh ();
        }
    }

    

}