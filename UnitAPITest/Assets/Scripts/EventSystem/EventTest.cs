using MoreFun;
using UnityEngine;
using UnityEngine.Events;

public class EventTest : MonoBehaviour
{
    private UnityAction someListener;

    void Awake()
    {
        someListener = new UnityAction(SomeFunction);
    }

    void OnEnable()
    {
        EventManager.StartListening("test", someListener);
    }


    void OnDisable()
    {
        EventManager.StopListening("test", someListener);
    }

    private void SomeFunction()
    {
        MoreDebug.MoreLog("Some Function was called!");
    }
}



