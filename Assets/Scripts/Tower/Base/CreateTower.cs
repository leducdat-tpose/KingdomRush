using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum TowerType{
    None,
    Archer,
    Soldier,
    Artillery,
    Mage,
}
public class CreateTower : MonoBehaviour
{
    [SerializeField, ReadOnly]
    private int _towerLevel = 0;
    [SerializeField]
    private GameObject _containTower;
    [SerializeField]
    private InteractUI _optionUI;
    [SerializeField]
    private List<TowerDetail> _towerDetails;
    
    private void Start() {
        _towerLevel = 0;
        _containTower = transform.GetChild(1).gameObject;
        _optionUI = GetComponentInChildren<InteractUI>(includeInactive:true);
    }

    public void ChooseTower(TowerType type)
    {
        _towerLevel = 1;
        TowerDetail selectedTower = type switch
        {
            TowerType.Archer => _towerDetails[0],
            TowerType.Soldier => _towerDetails[1],
            TowerType.Artillery => _towerDetails[2],
            TowerType.Mage => _towerDetails[3],
            _ => _towerDetails[0],
        };
        TowerInfo selectedTowerInfo = selectedTower.TowerInfoList[_towerLevel - 1];
        GameObject newTower = Instantiate(selectedTowerInfo.Prefab, parent: _containTower.transform);
        Warrior warrior = newTower.GetComponentInChildren<Warrior>();
        warrior.SetDamageCause(selectedTowerInfo.DamageCause);
        _optionUI.transform.parent.gameObject.SetActive(false);
    }
}
