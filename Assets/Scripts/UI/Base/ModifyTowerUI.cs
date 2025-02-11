using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyTowerUI : InteractUI
{
    [SerializeField]
    private CreateTower _createTower;
    [SerializeField]
    private OptionTowerBtn[] _optionBtns = new OptionTowerBtn[4];
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
    }
    private void OnEnable() {
        var value = _createTower.TowerLevel == 0;
        foreach( OptionTowerBtn btn in _optionBtns) btn.gameObject.SetActive(value);
        _upgradeBtn.gameObject.SetActive(!value);
        _sellBtn.gameObject.SetActive(!value);
    }
    private void Start() {
        
    }
    public override void SetActive()
    {
        if(_createTower.IsBusy) return;
        this.gameObject.SetActive(true);
    }
}
