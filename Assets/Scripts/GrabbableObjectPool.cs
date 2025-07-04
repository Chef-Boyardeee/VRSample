using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;
    public int poolSize = 10;
    public Transform spawnPoint;
    public float spawnDelay = 0.5f;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
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
        for (int i = 0; i < poolSize; i++)
        {
            SpawnFromPool();
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void SpawnFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            obj.SetActive(true);

            // Reset physics
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            StartCoroutine(EnablePhysicsDelayed(rb));
        }
    }

    private IEnumerator EnablePhysicsDelayed(Rigidbody rb)
    {
        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = false;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
