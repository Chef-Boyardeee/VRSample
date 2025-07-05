using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;
    public int poolSize = 10;
    public Transform[] spawnPoints; // Support multiple spawn points
    public float spawnDelay = 0.5f;
    public Vector3 offsetStep = new Vector3(0, 0.15f, -0.2f); // Stagger position to avoid stacking

    private Queue<GameObject> pool = new Queue<GameObject>();
    private int[] spawnPointCounters;

    private void ResetObjectPositions()
    {
        // Reset all basketball positions regardless of their state
    }

    void Start()
    {
        // Add ResetObjectPositions to onTimerStartDelegate
        UIManager.onTimerStartDelegate += ResetObjectPositions;

        spawnPointCounters = new int[spawnPoints.Length];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }

        StartCoroutine(SpawnInitialObjects());
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

                // Offset to avoid stacking
                Vector3 offset = offsetStep * spawnPointCounters[spawnIndex];
                obj.transform.SetPositionAndRotation(spawnPoint.position + offset, spawnPoint.rotation);
                spawnPointCounters[spawnIndex]++;

                obj.SetActive(true);

                Rigidbody rb = obj.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;

                yield return new WaitForSeconds(0.1f);
                rb.isKinematic = false;

                // Alternate spawn points
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
        obj.transform.SetPositionAndRotation(spawnPoint.position + offset, spawnPoint.rotation);
        spawnPointCounters[spawnIndex]++;

        obj.SetActive(true);

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

}
