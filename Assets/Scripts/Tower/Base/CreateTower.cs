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
    // public UnityEvent<TowerType> CreateTowerEvent;
    [SerializeField]
    private GameObject _containTower;
    [SerializeField]
    private List<GameObject> _towerPrefabs;
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
        GameObject newTower = Instantiate(selectedTower);
        newTower.transform.SetParent(_containTower.transform);
    }
    public void testFunc(){}
}
