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

    // When player drops/releases the object
    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        // Start respawn countdown
        respawnCoroutine = StartCoroutine(ReturnToPoolAfterDelay(2f));
    }

    // When player picks up the object again
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

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        yield return new WaitForSeconds(0.1f);

        if (pool != null)
            pool.ReturnToPool(gameObject);
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
            pool.ReturnToPool(gameObject);
    }
}
