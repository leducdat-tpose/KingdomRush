using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public int GetTotalAmount()
    {
        return enemiesInfo.Sum(t => t.amount);
    }
}
public class SpawnEnemiesInfo : ScriptableObject
{
    public float TimeBetweenWaves;
    public float EnemiesPerSec;
    public int BaseEnemies;
    public float DiffScalingFactor;
    public List<Wave> waves = new List<Wave>();
    
}
