using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableUI : MonoBehaviour, ISelectHandler, IDeselectHandler,
IEventSystemHandler, IPointerDownHandler, IUpdateSelectedHandler
{
    private InteractUI _ui;

    private void Start() {
        _ui = GetComponentInChildren<InteractUI>(includeInactive:true);
    }
    public void OnDeselect(BaseEventData eventData)
    {
        _ui.gameObject.SetActive(false);
        Debug.Log("OnDeselect");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
        return;
        //Crucial
        EventSystem.current.SetSelectedGameObject(gameObject, eventData);
    }

    public void OnSelect(BaseEventData eventData)
    {
        _ui.gameObject.SetActive(true);
        Debug.Log("OnSelect");
    }

    //Run the code in Update when this obj is selected
    public void OnUpdateSelected(BaseEventData eventData)
    {
    }
}
