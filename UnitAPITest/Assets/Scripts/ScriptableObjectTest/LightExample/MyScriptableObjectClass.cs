using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Light/MyScriptableObject", order = 1)]
public class MyScriptableObjectClass : ScriptableObject {

    public string objectName = "New MyScriptableObject";
    public bool colorIsRandom = false;
    public Color thisColor = Color.white;
    public Vector3[] spawnPoints;
}
