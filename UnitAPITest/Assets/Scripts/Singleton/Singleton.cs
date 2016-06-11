using System;
using UnityEngine;
namespace Next.Core.Singleton
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static bool _dontdestroyonload = true;
        private static T _instance;
        private static bool _instantiated;
        public static T Instance {
            get
            {
                if (_instantiated)
                {
                    return _instance;
                }

                var type = typeof(T);
                var objects = FindObjectsOfType<T>();

                if (objects.Length > 0)
                {
                    // there is at least one Object of the type we want inside the scene
                    _instance = objects[0];
                    if (objects.Length > 1)
                    {
                        Debug.LogWarning("There is more than one instance of Singleton of type \"" + type + "\". Keeping the first. Destroying the others.");
                        for (int i = 1; i < objects.Length; i++)
                        {
                            DestroyImmediate(objects[i].gameObject);
                        }
                    }
                    _instantiated = true;

                    if (_dontdestroyonload)
                    {
                        DontDestroyOnLoad(_instance.gameObject);
                    }
                    return _instance;
                }

                // if the Object is not inside the scene yet, try to create it
                var attribute = Attribute.GetCustomAttribute(type, typeof(PrefabAttribute)) as PrefabAttribute;
                if (null == attribute)
                {
                    Debug.LogError("There is no prefab Attribute for Singleton of type \"" + type + "\".");
                    return null;
                }
                var prefabName = attribute.Name;
                if (String.IsNullOrEmpty(prefabName))
                {
                    Debug.LogError("Prefab name is empty for singleton of type \"" + type + "\".");
                    return null;
                }

                var gameObject = Instantiate(Resources.Load<GameObject>(prefabName)) as GameObject;
                if (null == gameObject)
                {
                    Debug.LogError("Could not find Prefab \"" + prefabName + "\" on Resources for Singleton of type \"" + type + "\".");
                    return null;
                }
                gameObject.name = type.ToString();
                _instance = gameObject.AddComponent<T>();
                if (null == _instance)
                {
                    Debug.LogWarning("There wasn't a component of type \"" + type + "\" inside prefab \"" + prefabName + "\". Creating one.");
                    _instance = gameObject.AddComponent<T>();
                }

                if (attribute.Persistent)
                {
                    DontDestroyOnLoad(_instance.gameObject);
                }
                
                return _instance;
            }

            private set
            {
                _instance = value;
                _instantiated = value != null;
            }
        }

        protected void OnDestroy()
        {
            _instantiated = false;
        }
    }
}
