using System.Collections;
using UnityEngine;
public class SelfDestructcs : MonoBehaviour
{
    public GameObject explosion;

    private float shake = 0.2f;
    private AudioSource audioSource;

    public void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Destroy()
    {
        EventManager.StopListening("Destroy", Destroy);
        EventManager.StopListening("Junk", Destroy);
        StartCoroutine(DestroyNow());
    }

    public void OnDisable()
    {
        EventManager.StopListening("Destroy", Destroy);
    }

    public void OnEnable()
    {
        EventManager.StartListening("Destroy", Destroy);
    }

    IEnumerator DestroyNow()
    {
        yield return new WaitForSeconds(Random.Range(0.0f, 1.0f));
        audioSource.pitch = Random.Range(0.75f, 1.75f);
        audioSource.Play();
        float startTime = 0;
        float shakeTime = Random.Range(1.0f, 3.0f);
        while (startTime < shakeTime)
        {
            transform.Translate(Random.Range(-shake, shake), 0, Random.Range(-shake, shake));
            transform.Rotate(0, Random.Range(-shake * 100, shake * 100), 0);
            startTime += Time.deltaTime;
            yield return null;
        }
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
