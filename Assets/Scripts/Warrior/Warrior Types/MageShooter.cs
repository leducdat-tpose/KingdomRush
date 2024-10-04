using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageShooter : Warrior
{
    protected override void Update()
    {
        Render();
        ShootingProjectTile();
    }

    protected override void UpgradeSolider(int towerLevel)
    {
    }
}
