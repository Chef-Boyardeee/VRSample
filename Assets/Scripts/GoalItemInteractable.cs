using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GoalItemInteractable : XRGrabInteractable
{
    [SerializeField] private GoalItem item;
    private MeshRenderer myRend;

    public void OnAcceptItem()
    {
        Debug.Log("Right item.");
    }

    public void OnRejectItem()
    {
        ChangeMaterial();
    }

    public void OnRemoveItem()
    {
        ResetMaterial();
    }

    private void ChangeMaterial()
    {

    }

    private void ResetMaterial()
    {

    }

    public void ResetItem()
    {
        StartCoroutine("ResetCoroutine");
    }

    public IEnumerator ResetCoroutine()
    {
        Debug.Log("ResetCoroutine.");
        GetComponent<Collider>().enabled = false;
        yield return new WaitForSeconds(0.4f);
        GetComponent<Collider>().enabled = true;
    }

    public GoalItem GetItem()
    {
        return item;
    }
}
