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

    protected override void Awake()
    {
        base.Awake();
        StartCoroutine("AwakeCoroutine");
    }

    private IEnumerator AwakeCoroutine()
    {
        selectEntered.AddListener(AcceptItem);
        yield return null;
    }

    private void AcceptItem(SelectEnterEventArgs args)
    {
        Debug.Log("Item Accepted.");
        if(onAcceptItem != null && args.interactableObject.transform.gameObject.GetComponent<GoalItemInteractable>().GetItem() == requiredItem)
        {
            onAcceptItem();
        }
    }

    public GoalItem GetItem()
    {
        return requiredItem;
    }
}
