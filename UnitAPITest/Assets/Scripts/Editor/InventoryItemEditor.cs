using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using MoreFun;
public class InventoryItemEditor : EditorWindow
{
    public InventoryItemList inventoryItemList;
    private int viewIndex = 1;

    [MenuItem("Window/Inventory Item Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(InventoryItemEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjetPath");
            inventoryItemList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(InventoryItemList)) as InventoryItemList;
        }
    }

    void OnGUI()
    {
        #region First Line
        GUILayout.BeginHorizontal();
        GUILayout.Label("Inventory Item Editor", EditorStyles.boldLabel);
        if (null != inventoryItemList)
        {
            if (GUILayout.Button("Show Item List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = inventoryItemList;
            }
        }
        if (GUILayout.Button("Open Item List"))
        {
            OpenItemList();
        }
        if (GUILayout.Button("New Item List"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = inventoryItemList;
        }
        GUILayout.EndHorizontal();
        #endregion

        #region Second Line
        if (null == inventoryItemList)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Item List", GUILayout.ExpandWidth(false)))
            {
                CreateNewItemList();
            }
            if (GUILayout.Button("Open Existing Item List", GUILayout.ExpandWidth(false)))
            {
                OpenItemList();
            }
            GUILayout.EndHorizontal();
        }
        #endregion

        GUILayout.Space(20);

        #region Operate Data
        if (null != inventoryItemList)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex > 1)
                {
                    viewIndex--;
                }
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (viewIndex < inventoryItemList.itemList.Count)
                {
                    viewIndex++;
                }
            }
            GUILayout.Space(60);
            if (GUILayout.Button("Add Item", GUILayout.ExpandWidth(false)))
            {
                AddItem();
            }
            if (GUILayout.Button("Delete Item", GUILayout.ExpandWidth(false)))
            {
                DeleteItem(viewIndex - 1);
            }
            GUILayout.EndHorizontal();

            if (null == inventoryItemList.itemList)
            {
                MoreDebug.MoreLog("wtf");
            }
            if (inventoryItemList.itemList.Count > 0)
            {
                GUILayout.BeginHorizontal();
                viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Item", viewIndex, GUILayout.ExpandWidth(false)), 1, inventoryItemList.itemList.Count);
                EditorGUILayout.LabelField("of  " + inventoryItemList.itemList.Count.ToString() + " items", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                inventoryItemList.itemList[viewIndex - 1].itemName = EditorGUILayout.TextField("Item Name", inventoryItemList.itemList[viewIndex - 1].itemName as string);
                inventoryItemList.itemList[viewIndex - 1].itemIcon = EditorGUILayout.ObjectField("Item Icon", inventoryItemList.itemList[viewIndex - 1].itemIcon, typeof(Texture2D), false) as Texture2D;
                inventoryItemList.itemList[viewIndex - 1].itemObject = EditorGUILayout.ObjectField("Item Object", inventoryItemList.itemList[viewIndex - 1].itemObject, typeof(Rigidbody), false) as Rigidbody;

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                inventoryItemList.itemList[viewIndex - 1].isUnique = (bool)EditorGUILayout.Toggle("Unique", inventoryItemList.itemList[viewIndex - 1].isUnique, GUILayout.ExpandWidth(false));
                inventoryItemList.itemList[viewIndex - 1].isIndestructible = (bool)EditorGUILayout.Toggle("Indestructible", inventoryItemList.itemList[viewIndex - 1].isIndestructible, GUILayout.ExpandWidth(false));
                inventoryItemList.itemList[viewIndex - 1].isQuestItem = (bool)EditorGUILayout.Toggle("QuestItem", inventoryItemList.itemList[viewIndex - 1].isQuestItem, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                inventoryItemList.itemList[viewIndex - 1].isStackable = (bool)EditorGUILayout.Toggle("Stackable", inventoryItemList.itemList[viewIndex - 1].isStackable, GUILayout.ExpandWidth(false));
                inventoryItemList.itemList[viewIndex - 1].destroyOnUse = (bool)EditorGUILayout.Toggle("Destroy On Use", inventoryItemList.itemList[viewIndex - 1].destroyOnUse, GUILayout.ExpandWidth(false));
                inventoryItemList.itemList[viewIndex - 1].encumbranceValue = EditorGUILayout.FloatField("Encumberance", inventoryItemList.itemList[viewIndex - 1].encumbranceValue, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);
            }
            else
            {
                GUILayout.Label("This Inventory List is Empty.");
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(inventoryItemList);
        }
        #endregion
    }

    private void OpenItemList()
    {
        if (true)
        {
            
        }
    }

    private void CreateNewItemList()
    {

    }

    private void AddItem()
    {

    }

    private void DeleteItem(int index)
    {

    }
}
