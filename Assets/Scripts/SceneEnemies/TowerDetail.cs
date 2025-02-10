using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TowerInfo{
    public int Level;
    public GameObject Prefab;
    public int Cost;
    public float DamageCause;
}


[CreateAssetMenu(fileName ="TowerDetail", menuName ="ScriptableObject/TowerDetail")]
public class TowerDetail : ScriptableObject
{
    public List<TowerInfo> TowerInfoList;
}
