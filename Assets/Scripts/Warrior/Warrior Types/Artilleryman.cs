using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artilleryman : Warrior
{
    protected override void Start()
    {
        base.Start();
        LoadingProjectilePosition = transform.parent.transform.position;
    }
    protected override void GetTower()
    {
        tower = gameObject.transform.parent.gameObject.transform.parent.gameObject;
        _tower = tower.GetComponent<Tower>();
    }
}
