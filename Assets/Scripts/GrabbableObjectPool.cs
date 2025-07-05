using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;
    public int poolSize = 10;
    public Transform[] spawnPoints;
    public float spawnDelay = 0.5f;
    public Vector3 offsetStep = new Vector3(0, 0.15f, -0.2f);

    private Queue<GameObject> pool = new Queue<GameObject>();
    private List<GameObject> allObjects = new List<GameObject>();
    private int[] spawnPointCounters;

    private void Start()
    {
        UIManager.onTimerStartDelegate += OnPlayStart;

        spawnPointCounters = new int[spawnPoints.Length];

        // Do NOT spawn anything at start
    }

    private IEnumerator SpawnInitialObjects()
    {
        int spawnIndex = 0;

        for (int i = 0; i < poolSize; i++)
        {
            if (pool.Count > 0)
            {
                GameObject obj = pool.Dequeue();

                Transform spawnPoint = spawnPoints[spawnIndex];
                Vector3 offset = offsetStep * spawnPointCounters[spawnIndex];
                obj.transform.SetPositionAndRotation(spawnPoint.position + offset, spawnPoint.rotation);
                spawnPointCounters[spawnIndex]++;

                obj.SetActive(true);

                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                yield return new WaitForSeconds(0.1f);
                rb.isKinematic = false;

                spawnIndex = (spawnIndex + 1) % spawnPoints.Length;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    public void RespawnObject(GameObject obj)
    {
        StartCoroutine(RespawnAfterDelay(obj, 3f));
    }

    private IEnumerator RespawnAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];
        Vector3 offset = offsetStep * spawnPointCounters[spawnIndex];
        spawnPointCounters[spawnIndex]++;

        obj.transform.SetPositionAndRotation(spawnPoint.position + offset, spawnPoint.rotation);

        obj.SetActive(true);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnPlayStart()
    {
        // Destroy all existing basketballs (if any)
        foreach (GameObject obj in allObjects)
        {
            if (obj != null)
                Destroy(obj);
        }

        allObjects.Clear();
        pool.Clear();
        spawnPointCounters = new int[spawnPoints.Length];

        // Create new objects
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
            allObjects.Add(obj);
        }

        StartCoroutine(SpawnInitialObjects());
    }

    private void OnDestroy()
    {
        UIManager.onTimerStartDelegate -= OnPlayStart;
    }
}
