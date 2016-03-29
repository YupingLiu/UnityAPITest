using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using MoreFun;
using System.Collections.Generic;

namespace MoreFun
{
    public class AssetPreview
    {

        [MenuItem("Assets/Preview GameObject")]
        public static void PreviewGameObject()
        {
            PreviewGameObject(Selection.assetGUIDs);
        }

        private static void PreviewGameObject(string[] guid)
        {
            List<string> paths = new List<string>();
            List<GameObject> prefabs = new List<GameObject>();
            foreach(string oneGuid in guid)
            {
                string onePath = AssetDatabase.GUIDToAssetPath(oneGuid);

                Object oneObj = AssetDatabase.LoadAssetAtPath(onePath, typeof(GameObject));
                GameObject onePrefab = oneObj as GameObject;
                if(onePrefab != null)
                {
                    prefabs.Add(onePrefab);
                }
                else
                {
                    paths.Add(onePath);
                }
            }

            if(paths.Count > 0)
            {
                string[] guids = null;
                guids = AssetDatabase.FindAssets("t:prefab", paths.ToArray());

                if(null != guids)
                {
                    foreach(string oneGuid in guids)
                    {
                        string onePath = AssetDatabase.GUIDToAssetPath(oneGuid);
                        Object oneObj = AssetDatabase.LoadAssetAtPath(onePath, typeof(GameObject));
                        GameObject onePrefab = oneObj as GameObject;
                        if(null != onePrefab)
                        {
                            prefabs.Add(onePrefab);
                        }
                    }
                }
            }


            foreach(GameObject onePrefab in prefabs)
            {
                GameObject go = PrefabUtility.InstantiatePrefab(onePrefab) as GameObject;
                PostProcessPreview(go);
            }
        }

        private static void PostProcessPreview(GameObject goPreview)
        {

            GameObject goPreviewRootFolder = GameObject.Find("PreviewRootFolder");
            if(null == goPreviewRootFolder)
            {
                goPreviewRootFolder = new GameObject();
                goPreviewRootFolder.name = "PreviewRootFolder";
            }

            GameObject folder = null;

            RectTransform rt = goPreview.GetComponentInChildren<RectTransform>();
            if(null != rt)
            {
                Canvas canvas = goPreview.GetComponent<Canvas>();
                if(null == canvas)
                {
                    canvas = CreateCanvas();
                    rt.SetParent(canvas.transform, false);

                    folder = canvas.gameObject;
                }

                List<GameObject> lstRootGo = GameObjectUtil.GetRootGameObjects();
                bool hasEventSystemInHierachy = false;
                for(int i = 0; i < lstRootGo.Count; ++i)
                {
                    EventSystem[] lstEs = lstRootGo[i].GetComponentsInChildren<EventSystem>();
                    if(null != lstEs && 0 < lstEs.Length)
                    {
                        hasEventSystemInHierachy = true;
                        break;
                    }
                }

                if(false == hasEventSystemInHierachy)
                {
                    GameObject goEs = new GameObject();
                    goEs.name = "EventSystem";
                    goEs.AddComponent<EventSystem>();
                    goEs.AddComponent<TouchInputModule>();
                    goEs.AddComponent<StandaloneInputModule>();
                    goEs.transform.SetParent(goPreviewRootFolder.transform, false);

                }
            }

            Transform t = goPreview.GetComponent<Transform>();
            if(null == folder)
            {
                folder = new GameObject();
            }

            folder.name = goPreview.name + "Folder";
            t.SetParent(folder.transform, false);

            folder.transform.SetParent(goPreviewRootFolder.transform, false);

            Selection.activeObject = goPreview;
        }

        [MenuItem("MoreFun/UI/CreateCanvas")]
        public static Canvas CreateCanvas()
        {
            int uiLayer = LayerMask.NameToLayer("UI");
            int popupLayer = LayerMask.NameToLayer("Popup");
            int messgaeLayer = LayerMask.NameToLayer("MessageBox");

            GameObject camObj = new GameObject();
            camObj.name = "CanvasCamera";
            camObj.layer = uiLayer;
            camObj.transform.position = new Vector3(0.0f, 0.0f, -600.0f);
            Camera cam = camObj.AddComponent<Camera>();
            //cam.transform.localPosition = new Vector3(0.0f, 0.0f, -600.0f);
            cam.orthographic = true;
            cam.orthographicSize = 3.2f;
            cam.useOcclusionCulling = false;
            cam.clearFlags = CameraClearFlags.Depth;
            cam.cullingMask = (1 << uiLayer | 1 << popupLayer | 1 << messgaeLayer);//(LayerMask.GetMask("UI") | LayerMask.GetMask("Popup"));

            GameObject canvasObj = new GameObject();
            canvasObj.name = "Canvas";
            canvasObj.layer = uiLayer;
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = cam;
            CanvasScaler canvasScaler = canvasObj.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1136, 640);
            GraphicRaycaster graphicRaycaster = canvasObj.AddComponent<GraphicRaycaster>();


            camObj.transform.SetParent(canvasObj.transform, false);

            return canvas;
        }
    }

}