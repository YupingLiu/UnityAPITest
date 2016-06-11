using UnityEngine;

public class EventTriggerTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.TriggerEvent("test");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            EventManager.TriggerEvent("Spawn");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            EventManager.TriggerEvent("Destroy");
        }
    }
}
