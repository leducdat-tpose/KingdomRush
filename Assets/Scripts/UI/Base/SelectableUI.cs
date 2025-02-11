using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableUI : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerDownHandler
, IPointerEnterHandler, IPointerExitHandler
{
    private bool _isEnter = false;
    private InteractUI _ui;
    private Selectable _selectable;

    private void Start() {
        _ui = GetComponentInChildren<InteractUI>(includeInactive:true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if(_isEnter == true) return;
        _ui.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
        return;
        _ui.SetActive();
    }

    public void OnSelect(BaseEventData eventData)
    {
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isEnter = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isEnter = false;
    }
}
