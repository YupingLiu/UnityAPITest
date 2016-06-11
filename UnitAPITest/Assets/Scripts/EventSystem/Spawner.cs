using MoreFun;
using UnityEngine;
using UnityEngine.Events;
public class Spawner : MonoBehaviour
{
    public int spawnCount;
    [Range(1, 100)]
    public int spawnSize = 1;
    public float minionOffset = 1;
    public GameObject minion;

    public UnityAction spawnListener;

    public void Awake()
    {
        spawnListener = new UnityAction(Spawn);
    }

    public void OnDisable()
    {
        EventManager.StartListening("Spawn", spawnListener);
    }

    public void OnEnable()
    {
        EventManager.StopListening("Spawn", spawnListener);
    }

    private void Spawn()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition();
            Quaternion spawnRotation = Quaternion.identity;
            spawnRotation.eulerAngles = new Vector3(0, Random.Range(0, 360f));
            if (spawnPosition != Vector3.zero)
            {
                Instantiate(minion, spawnPosition, spawnRotation);
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        Vector3 spawnPosition = new Vector3();
        float startTime = Time.realtimeSinceStartup;
        bool test = false;
        while (!test)
        {
            Vector2 spawnPositionRaw = Random.insideUnitCircle * spawnSize;
            spawnPosition = new Vector3(spawnPositionRaw.x, minionOffset, spawnPositionRaw.y);
            test = !Physics.CheckSphere(spawnPosition, 0.75f);
            if (Time.realtimeSinceStartup - startTime > 0.5f)
            {
                MoreDebug.MoreLog("Time out placing Minion!");
                return Vector3.zero;
            }
        }
        return spawnPosition;
    }
}

