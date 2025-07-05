using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabbableObject : XRGrabInteractable
{
    private Rigidbody rb;
    private GrabbableObjectPool pool;
    private Coroutine respawnCoroutine;

    private bool isInsideZone = true;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        pool = FindObjectOfType<GrabbableObjectPool>();
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);

        if (!isInsideZone && respawnCoroutine == null)
        {
            respawnCoroutine = StartCoroutine(RespawnAfterDelay(3f));
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (respawnCoroutine != null)
        {
            StopCoroutine(respawnCoroutine);
            respawnCoroutine = null;
        }
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        pool?.RespawnObject(gameObject);
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

        pool?.RespawnObject(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ValidZone"))
        {
            isInsideZone = true;

            if (respawnCoroutine != null)
            {
                StopCoroutine(respawnCoroutine);
                respawnCoroutine = null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ValidZone"))
        {
            isInsideZone = false;

            // Only start timer if not being held
            if (!isSelected && respawnCoroutine == null)
            {
                respawnCoroutine = StartCoroutine(RespawnAfterDelay(3f));
            }
        }
    }
}
