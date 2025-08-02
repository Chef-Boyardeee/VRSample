using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GoalPillar : XRSocketInteractor
{
    [SerializeField] private GoalItem requiredItem;
    [SerializeField] private GameObject snapVolume;

    private bool isUsed;

    public delegate void OnAcceptItem();
    public OnAcceptItem onAcceptItem;

    public delegate void OnRejectItem();
    public OnRejectItem onRejectItem;

    public delegate void OnRemoveItem();
    public OnRemoveItem onRemoveItem;

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine("AwakeCoroutine");
    }

    private IEnumerator AwakeCoroutine()
    {
        selectEntered.AddListener(AcceptItem);
        selectEntered.AddListener(RejectItem);
        selectExited.AddListener(RemoveItem);
        yield return null;
    }

    private void AcceptItem(SelectEnterEventArgs args)
    {
        GoalItemInteractable item = args.interactableObject.transform.gameObject.GetComponent<GoalItemInteractable>();
        Debug.Log("Accept Item Call.");
        if (onAcceptItem != null && item.GetItem() == requiredItem && !isUsed)
        {
            Debug.Log("Accept Item If Check.");
            item.OnAcceptItem();
            onAcceptItem();
        }
    }

    private void RejectItem(SelectEnterEventArgs args)
    {
        GoalItemInteractable item = args.interactableObject.transform.gameObject.GetComponent<GoalItemInteractable>();
        Debug.Log("Reject Item Call.");
        if(onRejectItem != null && item.GetItem() != requiredItem)
        {
            Debug.Log("Reject Item If Check.");
            item.OnRejectItem();
            onRejectItem();
        }
    }

    private void RemoveItem(SelectExitEventArgs args)
    {
        Debug.Log("Remove Item Call.");
        GoalItemInteractable item = args.interactableObject.transform.gameObject.GetComponent<GoalItemInteractable>();
        item.OnRemoveItem();
    }

    public GoalItem GetItem()
    {
        return requiredItem;
    }

    public void SetIsUsed(bool b)
    {
        isUsed = b;
    }
}
