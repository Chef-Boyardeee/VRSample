using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbableObject : XRGrabInteractable
{
    private Rigidbody rb;
    private GrabbableObjectPool pool;
    private Coroutine respawnCoroutine;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        pool = FindObjectOfType<GrabbableObjectPool>();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Start respawn countdown if not already pending
        if (respawnCoroutine == null)
            respawnCoroutine = StartCoroutine(RespawnAfterDelay(2f));
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        // Cancel respawn if it's pending
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
            respawnCoroutine = null;
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset physics
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        // Respawn the object via the pool
        if (pool != null)
            pool.RespawnObject(gameObject);

        respawnCoroutine = null;
    }

    public void OnScored()
    {
        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
            respawnCoroutine = null;
        }

        StartCoroutine(ScoreRoutine());
    }

    private IEnumerator ScoreRoutine()
    {
        yield return new WaitForSeconds(1f);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        if (pool != null)
            pool.RespawnObject(gameObject);
    }
}
