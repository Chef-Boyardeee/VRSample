using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GoalPillar : XRSocketInteractor
{
    [SerializeField] private GoalItem requiredItem;
    [SerializeField] private GameObject snapVolume;

    public delegate void OnAcceptItem();
    public OnAcceptItem onAcceptItem;

    public delegate void OnRejectItem();
    public OnRejectItem onRejectItem;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine("AwakeCoroutine");
    }

    private IEnumerator AwakeCoroutine()
    {
        selectEntered.AddListener(AcceptItem);
        selectEntered.AddListener(RejectItem);
        yield return null;
    }

    private void AcceptItem(SelectEnterEventArgs args)
    {
        if(onAcceptItem != null && args.interactableObject.transform.gameObject.GetComponent<GoalItemInteractable>().GetItem() == requiredItem)
        {
            onAcceptItem();
        }
    }

    private void RejectItem(SelectEnterEventArgs args)
    {
        if(onRejectItem != null && args.interactableObject.transform.gameObject.GetComponent<GoalItemInteractable>().GetItem() != requiredItem)
        {
            onRejectItem();
        }
    }

    public GoalItem GetItem()
    {
        return requiredItem;
    }
}
