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
        gameObject.GetComponent<MeshRenderer>().material = item.testMaterial;
    }

    private void ResetMaterial()
    {
        gameObject.GetComponent<MeshRenderer>().material = item.itemMaterial;
    }

    public GoalItem GetItem()
    {
        return item;
    }
}
