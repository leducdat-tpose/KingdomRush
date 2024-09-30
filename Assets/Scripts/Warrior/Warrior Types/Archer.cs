using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Warrior
{
    protected override void Update()
    {
        Render();
        ShootingProjectTile();
    }
}
