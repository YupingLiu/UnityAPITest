using UnityEngine;

public class EventTriggerTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManager.TriggerEvent("test");
        }
    }
}
