using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SoldierTower : Tower
{
    [Header("References")]
    [SerializeField]
    private GameObject soldierPrefab;
    [SerializeField]
    private List<Soldier> soldiers;
    [Header("Attributes")]
    [SerializeField]
    private int _soldiersCapacity = 4;

    [SerializeField] private Vector2 _markPosition;
    [SerializeField]
    private int _soldiersCount;
    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < _soldiersCapacity; i++)
        {
            Soldier newSoldier = PoolingObject.Instance.GetObject(soldierPrefab).GetComponent<Soldier>();
            newSoldier.transform.position = transform.position;
            newSoldier.transform.SetParent(transform);
            soldiers.Add(newSoldier);
            newSoldier.gameObject.SetActive(true);
        }
        _soldiersCount = soldiers.Count;
    }


    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ChangeStandPosition(_markPosition);
        }
    }

    protected override void UpdateCurrentTarget()
    {
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
    }
}
