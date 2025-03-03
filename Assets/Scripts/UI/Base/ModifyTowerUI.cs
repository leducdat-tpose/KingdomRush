using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyTowerUI : InteractUI
{
    [SerializeField]
    private CreateTower _createTower;
    [SerializeField]
    private GameObject _rangeUI;
    [SerializeField]
    private OptionTowerBtn[] _optionBtns = new OptionTowerBtn[4];
    [SerializeField]
    private OptionTowerBtn _orderMovingBtn;
    [SerializeField]
    private OptionTowerBtn _upgradeBtn;
    [SerializeField]
    private OptionTowerBtn _sellBtn;

    public override void Initialise()
    {
        _createTower = transform.root.GetComponent<CreateTower>();
        if(!_createTower)
        {
            Debug.LogError("Can't find CreateTower!");
            return;
        }
        for(int i = 0; i < _optionBtns.Length; i++)
        {
            _optionBtns[i].SetTowerDetail(_createTower.GetTowerDetail(i));
            _optionBtns[i].Initialise();
        }
        _upgradeBtn.Initialise();
        _sellBtn.Initialise();
        _orderMovingBtn.Initialise();
    }
    private void OnEnable() {
        var value = _createTower.TowerLevel == 0;
        foreach( OptionTowerBtn btn in _optionBtns) btn.gameObject.SetActive(value);
        SetActiveRangeUI(!value);
        if(_rangeUI.activeSelf)
        {
            var range = _createTower.CurrentTowerDetail.TowerInfoList[_createTower.TowerLevel - 1].AttackRange;
            _rangeUI.transform.localScale = new Vector3(range/2, range/2, 0);
        }
        _upgradeBtn.gameObject.SetActive(!value);
        _sellBtn.gameObject.SetActive(!value);
        _orderMovingBtn.gameObject.SetActive(_createTower.TowerCanHaveOrder() != null);
    }
    private void OnDisable() {
        SetActiveRangeUI(false);
    }
    private void Start() {
        
    }
    public override void SetActive()
    {
        if(_createTower.IsBusy) return;
        this.gameObject.SetActive(true);
    }

    public void SetActiveRangeUI(bool option)
    {
        _rangeUI.SetActive(option);
    }

    public IEnumerator WaitingTurnOffRangeUI()
    {
        SetActiveRangeUI(true);
        yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
        SetActiveRangeUI(false);
    }
}
