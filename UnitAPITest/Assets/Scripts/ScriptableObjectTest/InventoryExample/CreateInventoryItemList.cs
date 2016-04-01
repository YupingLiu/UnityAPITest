using UnityEditor;
using UnityEngine;
public class CreateInventoryItemList
{
    [MenuItem("Assets/Create/Inventory Item List")]
    public static InventoryItemList Create()
    {
        InventoryItemList asset = ScriptableObject.CreateInstance<InventoryItemList>();

        AssetDatabase.CreateAsset(asset, "Assets/Resources/InventoryData/InventoryItemList.asset");
        AssetDatabase.SaveAssets();
        return asset;
    }

}
