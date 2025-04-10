using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ResourceManagement : MonoBehaviour, IEvent
{
    [HideInInspector]
    public static UnityEvent<int> CollectResource;
    public int initialMoney = 400;
    private int _totalMoney=0;
    public int TotalMoney => _totalMoney;

    private void Awake() {
        _totalMoney = initialMoney;
        if(CollectResource == null) CollectResource = new UnityEvent<int>();
        CollectResource.AddListener(AddMoney);
    }

    private void Start() {
        
    }

    private void AddMoney(int amount)
    {
        if (_totalMoney < amount) return;
        _totalMoney += amount;
        EventBus<ResourceManagement>.Raise(this);
    }
}
