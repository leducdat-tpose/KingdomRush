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
    private int _soldiersCapacity = 1;

    [SerializeField] 
    private Vector2 _gatherPosition = new Vector3(-4,0,0);
    [SerializeField]
    private int _soldiersCount;
    public override void Initialise()
    {
        for (int i = 0; i < _soldiersCapacity; i++)
        {
            Soldier newSoldier = PoolingObject.Instance.GetObject(soldierPrefab).GetComponent<Soldier>();
            newSoldier.transform.position = _gatherPosition;
            newSoldier.transform.SetParent(transform);
            soldiers.Add(newSoldier);
            newSoldier.gameObject.SetActive(true);
        }
        _soldiersCount = soldiers.Count;
    }
    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
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
