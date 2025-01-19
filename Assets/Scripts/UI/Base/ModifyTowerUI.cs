using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyTowerUI : MonoBehaviour
{
    [SerializeField]
    private CreateTower _createTower;
    [SerializeField]
    private Button _selectArcherBtn;
    [SerializeField]
    private Button _selectSoldierBtn;
    [SerializeField]
    private Button _selectMageBtn;
    [SerializeField]
    private Button _selectArtilleryBtn;
    [SerializeField]
    private Button _upgradeBtn;
    [SerializeField]
    private Button _sellBtn;
    private void Awake() {
        _createTower = transform.root.GetComponent<CreateTower>();
        if(!_createTower)
        {
            Debug.LogError("Can't find CreateTower!");
            return;
        }
        _selectArcherBtn.onClick.AddListener(delegate{_createTower.ChooseTower(TowerType.Archer);});
        _selectSoldierBtn.onClick.AddListener(delegate{_createTower.ChooseTower(TowerType.Soldier);});
        _selectArtilleryBtn.onClick.AddListener(delegate{_createTower.ChooseTower(TowerType.Artillery);});
        _selectMageBtn.onClick.AddListener(delegate{_createTower.ChooseTower(TowerType.Mage);});
    }
    private void Start() {
        
    }
}
