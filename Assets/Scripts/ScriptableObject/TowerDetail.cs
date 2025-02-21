using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TowerInfo{
    public int Level;
    public GameObject Prefab;
    public int Cost;
    public float DamageCause;
    public int SellMoney;
}


[CreateAssetMenu(fileName ="TowerDetail", menuName ="ScriptableObject/TowerDetail")]
public class TowerDetail : ScriptableObject
{
    public TowerType Type;
    public Sprite MainIcon;
    public Sprite UnableIcon;
    public List<TowerInfo> TowerInfoList;
    public TowerInfo GetNextTowerInfo(int currentLevel)
    {
        if(currentLevel == TowerInfoList.Count)
            return null;
        return TowerInfoList[currentLevel];
    }
    public TowerInfo GetTowerInfo(int currentLevel) 
    => TowerInfoList[currentLevel - 1];
}
