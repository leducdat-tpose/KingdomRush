using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyMoveable
{
    float MoveSpeed {  get; set; }
    int PathIndex { get; set; }
    Vector3 TargetPosition {  get; set; }
    void UpdateTargetPosition();

}
