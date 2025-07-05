using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Sprite mainSprite;
    [SerializeField] private Sprite altSprite;

    private void Start()
    {
        mainSprite = this.gameObject.GetComponent<Image>().sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.gameObject.GetComponent<Image>().sprite = altSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.gameObject.GetComponent<Image>().sprite = mainSprite;
    }
}
