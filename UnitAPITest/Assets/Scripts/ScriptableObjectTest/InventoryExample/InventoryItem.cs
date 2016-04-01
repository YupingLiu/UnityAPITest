using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItem 
{
    public string itemName = "New Item";
    public Texture2D itemIcon = null;
    public Rigidbody itemObject = null;
    public bool isUnique = false;
    public bool isIndestructible = false;
    public bool isQuestItem = false;
    public bool isStackable = false;
    public bool destroyOnUse = false;
    public float encumbranceValue = 0;
}
