using UnityEngine;

public class DestroyByTimeExtended : MonoBehaviour
{
    public float lifeTime;

    public void Start()
    {
        Destroy(gameObject, lifeTime);
        GetComponent<AudioSource>().pitch = Random.Range(0.75f, 1.25f);
    }
}
