using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TowerType{
    None,
    Archer,
    Soldier,
    Artillery,
    Mage,
}
public class CreateTower : MonoBehaviour
{
    [SerializeField]
    private float _builtTime = 1f;
    public bool IsBusy{get; private set;} = false;
    [SerializeField]
    private int _towerLevel = 0;
    public int TowerLevel => _towerLevel;
    [SerializeField]
    private GameObject _containTower;
    [SerializeField]
    private Image _towerConstruct;
    [SerializeField]
    private InteractUI _optionUI;
    [SerializeField]
    private List<TowerDetail> _towerDetails;
    public TowerDetail CurrentTowerDetail{get; private set;}

    private Color _transparent = new Color(1,1,1,0);
    
    private void Start() {
        _towerLevel = 0;
        _containTower = transform.GetChild(1).gameObject;
        _optionUI = GetComponentInChildren<InteractUI>(includeInactive:true);
        _optionUI.Initialise();
    }

    public void ChooseTower(TowerType type)
    {
        TowerDetail selectedTower = type switch
        {
            TowerType.Archer => _towerDetails[0],
            TowerType.Soldier => _towerDetails[1],
            TowerType.Mage => _towerDetails[2],
            TowerType.Artillery => _towerDetails[3],
            _ => _towerDetails[0],
        };
        CurrentTowerDetail = selectedTower;
        var cost = -CurrentTowerDetail.GetNextTowerInfo(_towerLevel).Cost;
        ResourceManagement.CollectResource?.Invoke(cost);
        StartCoroutine(nameof(StartBuilding));
        _optionUI.transform.gameObject.SetActive(false);
    }

    public void UpgradeTower()
    {
        Destroy(_containTower.transform.GetChild(0).gameObject);
        var cost = -CurrentTowerDetail.GetNextTowerInfo(_towerLevel).Cost;
        ResourceManagement.CollectResource?.Invoke(cost);
        StartCoroutine(nameof(StartBuilding));
        _optionUI.transform.gameObject.SetActive(false);
    }

    private IEnumerator StartBuilding()
    {
        IsBusy = true;
        _towerConstruct.color = Color.white;
        yield return new WaitForSeconds(_builtTime);
        TowerInfo selectedTowerInfo = CurrentTowerDetail.GetNextTowerInfo(_towerLevel);
        GameObject newTower = Instantiate(selectedTowerInfo.Prefab, parent: _containTower.transform);
        yield return new WaitUntil(() => {
            newTower.GetComponentInChildren<Tower>().Initialise();
            return true;
        });
        // Warrior warrior = newTower.GetComponentInChildren<Warrior>();
        // warrior.SetDamageCause(selectedTowerInfo.DamageCause);
        _towerConstruct.color = _transparent;
        _towerLevel += 1;
        IsBusy = false;
    }

    public void SellTower()
    {   
        Destroy(_containTower.transform.GetChild(0).gameObject);
        var sellMoney = CurrentTowerDetail.GetTowerInfo(TowerLevel).SellMoney;
        ResourceManagement.CollectResource?.Invoke(sellMoney);
        _optionUI.transform.gameObject.SetActive(false);
        _towerConstruct.color = Color.white;
        _towerLevel = 0;
    }

    public void GiveOrder()
    {
        var selectedTower = TowerCanHaveOrder();
        _optionUI.transform.gameObject.SetActive(false);
        if(selectedTower == null) return;
        StartCoroutine(WaitALittleBit(selectedTower));
    }

    private IEnumerator WaitALittleBit(IOrderable selectedTower){
        yield return new WaitForEndOfFrame();
        selectedTower.SetOrder(true);
        StartCoroutine(_optionUI.gameObject.GetComponent<ModifyTowerUI>().WaitingTurnOffRangeUI());
    }

    public IOrderable TowerCanHaveOrder()
    {
        if(_containTower.transform.childCount == 0) return null;
        if(_containTower.transform.GetChild(0).GetChild(1).TryGetComponent<IOrderable>(out IOrderable selectedTower)) 
            return selectedTower;
        return null;
    }


    public TowerDetail GetTowerDetail(int ind) => _towerDetails[ind];
}
