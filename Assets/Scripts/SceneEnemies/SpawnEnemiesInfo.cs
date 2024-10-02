using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemiesInWave
{
    public GameObject prefab;
    public int amount;
}
[Serializable]
public class Wave
{
    public List<EnemiesInWave> enemiesInfo;
}
public class SpawnEnemiesInfo : ScriptableObject
{
    public List<Wave> waves = new List<Wave>();
}
