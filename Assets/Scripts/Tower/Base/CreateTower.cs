using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

public enum TowerType{
    Archer,
    Soldier,
    Artillery,
    Mage,
}
public class CreateTower : MonoBehaviour
{
    [SerializeField]
    private GameObject _containTower;
    [SerializeField]
    private InteractUI _optionUI;
    [SerializeField]
    private List<GameObject> _towerPrefabs;
    
    private void Start() {
        _containTower = transform.GetChild(1).gameObject;
        _optionUI = GetComponentInChildren<InteractUI>(includeInactive:true);
    }

    public void ChooseTower(TowerType type)
    {
        GameObject selectedTower = type switch
        {
            TowerType.Archer => _towerPrefabs[0],
            TowerType.Soldier => _towerPrefabs[1],
            TowerType.Artillery => _towerPrefabs[2],
            TowerType.Mage => _towerPrefabs[3],
            _ => _towerPrefabs[0],
        };
        GameObject newTower = Instantiate(selectedTower, parent: _containTower.transform);
        _optionUI.transform.parent.gameObject.SetActive(false);
    }
}
