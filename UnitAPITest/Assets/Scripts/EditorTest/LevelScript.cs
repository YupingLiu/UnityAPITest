using UnityEngine;

public class LevelScript : MonoBehaviour {

    public int experience;
    public int Level
    {
        get { return experience / 750; }
    }
}
