using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UseScriptableObject : MonoBehaviour {
    public MyScriptableObjectClass myScriptableObject;
    private List<Light> myLights;

	// Use this for initialization
	void Start () {
        myLights = new List<Light>();
        for (int i = 0; i < myScriptableObject.spawnPoints.Length; i++)
        {
            Vector3 spawn = myScriptableObject.spawnPoints[i];
            GameObject myLight = new GameObject("Light");
            Light light = myLight.AddComponent<Light>();
            myLight.transform.position = spawn;
            light.enabled = false;
            if (myScriptableObject.colorIsRandom)
            {
                light.color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));
            }
            else
            {
                light.color = myScriptableObject.thisColor;
            }
            light.intensity = 8.0f;
            myLights.Add(light);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            TurnOnOrOffLights();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            UpdateLights();
        }
	}

    private void TurnOnOrOffLights()
    {
        for (int i = 0; i < myLights.Count; i++)
        {
            myLights[i].enabled = !myLights[i].enabled;
        }
    }

    private void UpdateLights()
    {
        for (int i = 0; i < myLights.Count; i++)
        {
            myLights[i].color = new Color(Random.Range(0, 1.0f), Random.Range(0, 1.0f), Random.Range(0, 1.0f));
        }
    }
}
