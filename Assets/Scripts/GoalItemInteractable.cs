using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GoalItemInteractable : XRGrabInteractable
{
    [SerializeField] private GoalItem item;

    public void OnAcceptItem()
    {
        ChangeMaterial();
    }

    public void OnRejectItem()
    {
        Debug.Log("Wrong item.");
    }

    private void ChangeMaterial()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = item.testMaterial;
    }

    public GoalItem GetItem()
    {
        return item;
    }
}
