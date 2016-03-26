
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;
using System.IO;


namespace MoreFun
{
    public static class GameObjectUtil
    {
        private const string NotAvailable = "N/A";

        private static bool logDetail = false;
        private const string PrefLogDetail = "gameObjectLogDetail";
        public static void Initialize()
        {
            if(PlayerPrefs.HasKey(PrefLogDetail) && 0 != PlayerPrefs.GetInt(PrefLogDetail))
            {
                EnableLog(true);
            }
            else
            {
                EnableLog(false);
            }
        }

        public static void EnableLog(bool value)
        {
            logDetail = value;

            if(logDetail)
            {
                PlayerPrefs.SetInt(PrefLogDetail, 1);
            }
            else
            {
                PlayerPrefs.SetInt(PrefLogDetail, 0);
            }

            PlayerPrefs.Save();
        }

        /// <summary>
        /// SetActive() with additional caller's info.
        /// </summary>
        /// <param name="obj">the object to SetActive()</param>
        /// <param name="value"></param>
        /// <param name="caller">the object who make this call</param>
        public static void MoreSetActive(this GameObject obj, bool value, object caller)
        {
            #if LOG_DETAIL
            if(logDetail)
            {
                Debug.Log(MoreDebug.GetMoreLog(caller, 2, TryGetString(obj) + ".SetActive(" + value + ")"));
            }
            #endif
            if(obj.activeSelf != value)
            {
                obj.SetActive(value);
            }
        }
        
        /// <summary>
        /// SetActive(false) and SetActive(true) with additional caller's info.
        /// </summary>
        /// <param name="obj">the object to SetActive()</param>
        /// <param name="caller">the object who make this call</param>
        public static void MoreReactive(this GameObject obj, object caller)
        {
            #if LOG_DETAIL
            if(logDetail)
            {
                Debug.Log(MoreDebug.GetMoreLog(caller, 2, TryGetString(obj) + ".Reactive()"));
            }
            #endif
            if(obj.activeSelf == true)
            {
                obj.SetActive(false);
            }
            obj.SetActive(true);
        }

        /// <summary>
        /// set <c>enabled</c> with additional caller's info.
        /// </summary>
        /// <param name="obj">the object to set enabled</param>
        /// <param name="value"></param>
        /// <param name="caller">the object who make this call</param>
        public static void MoreSetEnabled(this Behaviour obj, bool value, object caller)
        {
            #if LOG_DETAIL
            if(logDetail)
            {
                Debug.Log(MoreDebug.GetMoreLog(caller, 2, TryGetString(obj) + ".enabled = " + value));
            }
            #endif
            if(obj.enabled != value)
            {
                obj.enabled = value;
            }
        }
        
        /// <summary>
        /// Instantiate() with additional caller's info.
        /// </summary>
        /// <param name="original">the object to be Destroy()</param>
        /// <param name="caller">the object who make this call</param>
        public static void MoreInstantiate(this object caller, UnityEngine.Object original)
        {
            #if LOG_DETAIL
            if(logDetail)
            {
                Debug.Log(MoreDebug.GetMoreLog(caller, 2, "Instantiate(" + TryGetString(original)+ ")"));
            }
            #endif
            UnityEngine.Object.Instantiate(original);
        }
        /// <summary>
        /// Instantiate() with additional caller's info.
        /// </summary>
        /// <param name="original">the object to be Destroy()</param>
        /// <param name="caller">the object who make this call</param>
        public static void MoreInstantiate(this object caller, UnityEngine.Object original, Vector3 position, Quaternion rotation)
        {
            #if LOG_DETAIL
            if(logDetail)
            {
                Debug.Log(MoreDebug.GetMoreLog(caller, 2, "Instantiate(" + TryGetString(original)+ ")"));
            }
            #endif
            UnityEngine.Object.Instantiate(original, position, rotation);
        }

        /// <summary>
        /// Destroy() with additional caller's info.
        /// </summary>
        /// <param name="value">the object to be Destroy()</param>
        /// <param name="caller">the object who make this call</param>
        public static void MoreDestroy(this object caller, UnityEngine.Object value)
        {
            #if LOG_DETAIL
            if(logDetail)
            {
                Debug.Log(MoreDebug.GetMoreLog(caller, 2, "Destroy(" + TryGetString(value)+ ")"));
            }
            #endif
            UnityEngine.Object.Destroy(value);
        }

        /// <summary>
        /// Destroy() with additional caller's info.
        /// </summary>
        /// <param name="value">the object to be Destroy()</param>
        /// <param name="t">time delayed to Destroy()</param>
        /// <param name="caller">the object who make this call</param>
        public static void MoreDestroy(this UnityEngine.Object caller, UnityEngine.Object value, float t)
        {
            #if LOG_DETAIL
            if(logDetail)
            {
                Debug.Log(MoreDebug.GetMoreLog(caller, 2, "Destroy(" + TryGetString(value) + ")"));
            }
            #endif
            UnityEngine.Object.Destroy(value, t);
        }
        
        private static string TryGetString(UnityEngine.Object obj)
        {
            if(obj.IsValid())
            {
                string ret = obj.ToString() + "(" + obj.GetInstanceID() + ")";
                GameObject go = obj as GameObject;
                if(go.IsValid())
                {
                    ret += "(transform=" + go.transform.GetInstanceID() + ")";
                }

                return ret;
            }
            else
            {
                return NotAvailable;
            }
        }

        /// <summary>
        /// 支持includeInactive的GetComponentInChildren
        /// </summary>
        /// <typeparam name="T">the type that you want to get</typeparam>
        /// <param name="go"></param>
        /// <param name="includeInactive">whether to get from an inactive child</param>
        /// <returns></returns>
        public static T GetComponentInChildren<T>(GameObject go, bool inactiveObject = false) where T : Component
        {
#if ENABLE_PROFILER
            Profiler.BeginSample("GetComponentInChildren");
#endif
            T ret = null;
            if (null != go)
            {
                if (false == inactiveObject)
                {
                    return go.GetComponentInChildren<T>();
                }
                else
                {
                    T[] arr = go.GetComponentsInChildren<T>(inactiveObject);
                    if(null != arr && 0 < arr.Length)
                    {
                        ret = arr[0];
                    }
                }
            }
            
            #if ENABLE_PROFILER
            Profiler.EndSample();
            #endif
            
            return ret;
        }


        /// <summary>
        /// get one component in all tagged gameObject
        /// </summary>
        /// <returns>The tag component.</returns>
        /// <param name="tag">Tag.</param>
        /// <param name="multipleObjects">If set to <c>true</c> multiple objects.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetTagComponent<T>(string tag, bool inactiveObject = false,
                                           bool multipleObjects = false) where T:Component
        {
            #if ENABLE_PROFILER
            Profiler.BeginSample("GetTagComponent");
            #endif
            T ret = null;

            if(false == multipleObjects)
            {
                GameObject go = GameObject.FindWithTag(tag);
                if(null != go)
                {
                    ret = go.GetComponent<T>();
                }
            }
            else
            {
                
                GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
                
                if(null != gos)
                {
                    GameObject oneGo;
                    Component oneCom;
                    
                    for(int i = 0; i < gos.Length; ++i)
                    {
                        oneGo = gos[i];
                        if(null != oneGo)
                        {
                            oneCom = oneGo.GetComponent<T>();
                            if(null != oneCom)
                            {
                                ret = oneCom as T;
                                break;
                            }
                        }
                    }
                }
            }
            
            
            #if ENABLE_PROFILER
            Profiler.EndSample();
            #endif
            return ret;
        }

        /// <summary>
        /// get one component in all tagged gameObjects and their children
        /// </summary>
        /// <returns>The tag component in children.</returns>
        /// <param name="tag">Tag.</param>
        /// <param name="includeInactive">If set to <c>true</c> include inactive.</param>
        /// <param name="multipleObjects">If set to <c>true</c> multiple objects.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static T GetTagComponentInChildren<T>(string tag, bool inactiveObject = false, bool multipleObjects = false) where T : Component
        {
            #if ENABLE_PROFILER
            Profiler.BeginSample("GetTagComponentInChildren");
            #endif
            T ret = null;

            if(false == multipleObjects)
            {
                GameObject go = GameObject.FindGameObjectWithTag(tag);
                
                ret = GetComponentInChildren<T>(go, inactiveObject);
            }
            else
            {
                GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);

                if(null != gos)
                {
                    GameObject oneGo;
                    Component oneCom;

                    for(int i = 0; i < gos.Length; ++i)
                    {
                        oneGo = gos[i];
                        if(null != oneGo)
                        {
                            oneCom = GetComponentInChildren<T>(oneGo, inactiveObject);
                            if(null != oneCom)
                            {
                                ret = oneCom as T;
                                break;
                            }
                        }
                    }
                }

            }
            
            #if ENABLE_PROFILER
            Profiler.EndSample();
            #endif
            return ret;
        }

        public static T[] GetTagComponentsInChildren<T>(string tag, bool inactiveObject = false) where T : Component
        {
            #if ENABLE_PROFILER
            Profiler.BeginSample("GetTagComponentInChildren");
            #endif
            T[] ret = null;
            GameObject[] gos = GameObject.FindGameObjectsWithTag(tag);
            if (null != gos)
            {
                List<T> tlist = new List<T>();

                GameObject oneGo;
                T oneCom;
                for(int i = 0; i < gos.Length; ++i)
                {
                    oneGo = gos[i];
                    if(null != oneGo)
                    {
                        oneCom = GetComponentInChildren<T>(oneGo, inactiveObject);
                        if(null != oneCom)
                        {
                            tlist.Add(oneCom);
                        }
                    }
                }

                ret = tlist.ToArray();
            }
            
            #if ENABLE_PROFILER
            Profiler.EndSample();
            #endif
            return ret;
        }

        public static List<GameObject> GetRootGameObjects()
        {
            return GetRootGameObjects(false);
        }
        
        public static List<GameObject> GetRootGameObjects(bool includeInactive)
        {
            List<GameObject> lstGo = new List<GameObject>();
            Transform[] arrTrans;
            if(false == includeInactive)
            {
                arrTrans = GameObject.FindObjectsOfType<Transform>();
            }
            else
            {
                arrTrans = Resources.FindObjectsOfTypeAll<Transform>();
            }

            for(int i = 0; i < arrTrans.Length; ++i)
            {
                if(arrTrans[i].parent == null)
                {
                    lstGo.Add(arrTrans[i].gameObject);
                }
            }
            
            return lstGo;
        }
        
        public static List<GameObject> GetAllGameObjects()
        {
            return GetAllGameObjects(false);
        }
        public static List<GameObject> GetAllGameObjects(bool includeInactive)
        {
            List<GameObject> lstGo = new List<GameObject>();
            Transform[] arrTrans;
            if(false == includeInactive)
            {
                arrTrans = GameObject.FindObjectsOfType<Transform>();
            }
            else
            {
                arrTrans = Resources.FindObjectsOfTypeAll<Transform>();
            }
            
            for(int i = 0; i < arrTrans.Length; ++i)
            {
                lstGo.Add(arrTrans[i].gameObject);
            }
            
            return lstGo;
        }
        
        public static bool IsTransformAncestor(Transform ancestor, Transform offspring)
        {
            if(null == ancestor || null == offspring)
            {
                return false;
            }

            Transform curr = offspring;
            do
            {
                if(curr == ancestor)
                {
                    return true;
                }

                curr = curr.parent;
            }while(null != curr);

            return false;
        }
        
        public static T Do<T>( GameObject host ) where T : Component
        {
            T com = host.GetComponent<T>();
            if (null == com)
            {
                com = host.AddComponent<T>();
            }
            return com;
        }
        
        static public T AddMissingComponent<T> (this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (comp == null)
            {
                #if UNITY_EDITOR
                if (!Application.isPlaying)
                    RegisterUndo(go, "Add " + typeof(T));
                #endif
                comp = go.AddComponent<T>();
            }
            return comp;
        }
        
        static public void RegisterUndo (UnityEngine.Object obj, string name)
        {
            #if UNITY_EDITOR
            #if UNITY_3_5 || UNITY_4_0 || UNITY_4_1 || UNITY_4_2
            UnityEditor.Undo.RegisterUndo(obj, name);
            #else
            UnityEditor.Undo.RecordObject(obj, name);
            #endif
            SetDirty(obj);
            #endif
        }
        
        /// <summary>
        /// Convenience function that marks the specified object as dirty in the Unity Editor.
        /// </summary>
        
        static public void SetDirty (UnityEngine.Object obj)
        {
            #if UNITY_EDITOR
            if (obj)
            {
                //if (obj is Component) Debug.Log(NGUITools.GetHierarchy((obj as Component).gameObject), obj);
                //else if (obj is GameObject) Debug.Log(NGUITools.GetHierarchy(obj as GameObject), obj);
                //else Debug.Log("Hmm... " + obj.GetType(), obj);
                UnityEditor.EditorUtility.SetDirty(obj);
            }
            #endif
        }
        
        public static void DebugSaveHierachyRoot()
        {
            DateTime now = DateTime.Now;
            string path = Application.persistentDataPath + "/debug/hierachy/hierachyRoot_" +
                now.Year.ToString() + now.Month.ToString().PadLeft(2, '0') + now.Day.ToString().PadLeft(2, '0') +
                    now.Hour.ToString().PadLeft(2, '0') + now.Minute.ToString().PadLeft(2, '0') + now.Second.ToString().PadLeft(2, '0') + ".txt";

            DebugSaveHierachyRoot(path);
        }

        
        public static void DebugSaveHierachyRoot(string path)
        {
            StringBuilder txtHierachy = new StringBuilder();
            txtHierachy.Append("Hierachy root, gameObject count is: ");

            List<GameObject> lstRoots = GetRootGameObjects();
            if(lstRoots.IsValid())
            {
                txtHierachy.AppendLine(lstRoots.Count.ToString());
                for(int i = 0; i < lstRoots.Count; ++i)
                {
                    GameObject oneGo = lstRoots[i].gameObject;
                    ToStringBuilder(oneGo, txtHierachy);
                    txtHierachy.AppendLine();
                }
            }
            else
            {
                txtHierachy.Append("null");
            }

            //MoreFileUtil.TrySaveFile(path, txtHierachy.ToString());
        }
        
        public static void DebugSaveHierachyAll()
        {
            DateTime now = DateTime.Now;
            string path = Application.persistentDataPath + "/debug/hierachy/hierachyAll_" +
                now.Year.ToString() + now.Month.ToString().PadLeft(2, '0') + now.Day.ToString().PadLeft(2, '0') +
                    now.Hour.ToString().PadLeft(2, '0') + now.Minute.ToString().PadLeft(2, '0') + now.Second.ToString().PadLeft(2, '0') + ".txt";

            DebugSaveHierachyAll(path);

            string pathFullPath = Application.persistentDataPath + "/debug/hierachy/hierachyAllFullPath_" +
                now.Year.ToString() + now.Month.ToString().PadLeft(2, '0') + now.Day.ToString().PadLeft(2, '0') +
                    now.Hour.ToString().PadLeft(2, '0') + now.Minute.ToString().PadLeft(2, '0') + now.Second.ToString().PadLeft(2, '0') + ".txt";
            DebugSaveHierachyFullPathAll(pathFullPath);

            //DebugSaveHierachyFullPathAll("D:/HirracyTeset");
        }
        public static void DebugSaveHierachyAll(string path)
        {
            StringBuilder txtHierachy = new StringBuilder();
            txtHierachy.Append("Hierachy root, gameObject count is: ");
            
            List<GameObject> lstRoots = GetRootGameObjects();

            if(lstRoots.IsValid())
            {
                object[] args = new object[] {txtHierachy};
                txtHierachy.AppendLine(lstRoots.Count.ToString());
                for(int i = 0; i < lstRoots.Count; ++i)
                {
                    GameObject oneGo = lstRoots[i].gameObject;
                    TraverseTransform(oneGo.transform, 0, TransformTraverseVisitor_Print, args);
                    txtHierachy.AppendLine();
                }
            }
            else
            {
                txtHierachy.Append("null");
            }

            //MoreFileUtil.TrySaveFile(path, txtHierachy.ToString());
        }

        public static void DebugSaveHierachyFullPathAll(string path)
        {
            StringBuilder txtHierachy = new StringBuilder();
            txtHierachy.Append("Hierachy root, gameObject count is: ");

            List<GameObject> lstRoots = GetRootGameObjects();

            if (lstRoots.IsValid())
            {
                object[] args = new object[] { txtHierachy };
                txtHierachy.AppendLine(lstRoots.Count.ToString());
                for (int i = 0; i < lstRoots.Count; ++i)
                {
                    GameObject oneGo = lstRoots[i].gameObject;
                    TraverseTransform(oneGo.transform, 0, TransformTraverseVisitor_PrintFullPath, args);
                    txtHierachy.AppendLine();
                }
            }
            else
            {
                txtHierachy.Append("null");
            }

            //MoreFileUtil.TrySaveFile(path, txtHierachy.ToString());
        }

        public static void TraverseTransform(Transform current, int currentDepth, TransformTraverseVisitor visitor, object[] args)
        {
            if(null != current)
            {
                visitor(current, currentDepth, args);
                if(0 < current.childCount)
                {
                    for(int i = 0; i < current.childCount; ++i)
                    {
                        TraverseTransform(current.GetChild(i), currentDepth + 1, visitor, args);
                    }
                }
            }
        }

        public delegate void TransformTraverseVisitor(Transform oneTransform, int currentDepth, object[] args);
        private static void TransformTraverseVisitor_Print(Transform oneTransform, int currentDepth, object[] args)
        {
            StringBuilder sb = args[0] as StringBuilder;
            for(int i = 0; i < currentDepth; ++i)
            {
                sb.Append("    ");
            }
            sb.Append(currentDepth);
            sb.Append(".");
            ToStringBuilder(oneTransform.gameObject, sb);
            sb.AppendLine();
        }
      

        static void GetStringFullPath(StringBuilder sb, Transform trans)
        {
            if (null == trans)
            {
                return;
            }
            else
            {
                if (null != trans.parent)
                {                    
                    GetStringFullPath(sb, trans.parent);
                    sb.Append("/");
                }
                sb.Append(trans.gameObject.name);
            }
        }

        private static void TransformTraverseVisitor_PrintFullPath(Transform oneTransform, int currentDepth, object[] args)
        {
            StringBuilder sb = args[0] as StringBuilder;
            GetStringFullPath(sb, oneTransform);
            sb.AppendLine();
        }

        public static StringBuilder ToStringBuilder(GameObject oneGo)
        {
            StringBuilder sb = new StringBuilder();
            return ToStringBuilder(oneGo, sb);
        }
        
        public static StringBuilder ToStringBuilder(GameObject oneGo, StringBuilder sb)
        {
            sb.Append(oneGo.name);
            sb.Append("{activeInHierarchy=");
            sb.Append(oneGo.activeInHierarchy);
            sb.Append(",activeSelf=");
            sb.Append(oneGo.activeSelf);
            sb.Append(",tag=");
            sb.Append(oneGo.tag);
            sb.Append(",layer=");
            sb.Append(oneGo.layer);
            sb.Append(",isStatic=");
            sb.Append(oneGo.isStatic);
            sb.Append(",hideFlags=");
            sb.Append(oneGo.hideFlags);
            sb.Append(",childCount=");
            sb.Append(oneGo.transform.childCount);
            sb.Append(",position=");
            sb.Append(oneGo.transform.position);
            sb.Append("}");
            
            return sb;
        }

        public static void SetChildText(GameObject this_gb, string child_path, string content)
        {
            Transform trans = this_gb.GetComponent<Transform>();
            if (null != trans)
            {
                Transform gb = trans.FindChild(child_path);
                if (null != gb)
                {
                    UnityEngine.UI.Text text = gb.GetComponent<UnityEngine.UI.Text>();
                    if (null != text)
                    {
                        text.text = content;
                    }
                }

            }
        }

        public static void SetButtonInteractable(GameObject this_gb, bool interactable)
        {
            if (null != this_gb)
            {
                UnityEngine.UI.Button bt = this_gb.GetComponent<UnityEngine.UI.Button>();
                if (null != bt)
                {
                    bt.interactable = interactable;
                }
            }
        }
    }
}
