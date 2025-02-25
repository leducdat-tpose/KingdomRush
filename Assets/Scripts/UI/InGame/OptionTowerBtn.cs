using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
internal enum OptionTowerType
{
    PurchaseTower,
    Upgrade,
    Sell,
    OrderMoving,
}

public class OptionTowerBtn : MonoBehaviour
{
    private int _price;
    [SerializeField]
    private CreateTower _createTower;
    [SerializeField]
    private TowerDetail _towerDetail;
    [SerializeField]
    private OptionTowerType _typeBtn;
    [SerializeField]
    private Sprite _mainSprite;
    [SerializeField]
    private Sprite _unableSprite;
    [SerializeField]
    private Sprite _lockSprite;
    [SerializeField]
    private Sprite _selectedSprite;
    [SerializeField]
    private Image _imgSlot;
    [SerializeField]
    private Button _btn;
    [SerializeField]
    private Text _priceTag;
    [SerializeField, ReadOnly]
    private bool _isSelected;
    [SerializeField, ReadOnly]
    private TowerType _type;
    private void Awake() {
    }
    private void Start() {
        
    }

    public void Initialise()
    {
        _createTower = transform.root.GetComponent<CreateTower>();
        _imgSlot = GetComponent<Image>();
        _priceTag = GetComponentInChildren<Text>();
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClick);
        _imgSlot.sprite = _mainSprite;
        if(_typeBtn == OptionTowerType.PurchaseTower)
        {
            _mainSprite = _towerDetail.MainIcon;
            _unableSprite = _towerDetail.UnableIcon;
            _type = _towerDetail.Type;
        }
    }

    private void OnEnable() {
        _isSelected = false;
        OnEnablePurchaseTowerBtn();
        OnEnableSellBtn();
        OnEnableUpgradeBtn();
        OnEnableOrderMovingBtn();
    }
    public void OnClick()
    {
        if(!_isSelected && _typeBtn != OptionTowerType.OrderMoving)
        {
            _isSelected = true;
            _imgSlot.sprite = _selectedSprite;
            return;
        }
        _isSelected = false;
        switch (_typeBtn)
        {
            case OptionTowerType.Upgrade:
                _createTower.UpgradeTower();
                break;
            case OptionTowerType.Sell:
                _createTower.SellTower();
                break;
            case OptionTowerType.OrderMoving:
                _createTower.GiveOrder();
                break;
            default:
                _createTower.ChooseTower(_type);
                break;
        }

    }
    public void SetType(TowerType type) => _type = type;

    public void SetTowerDetail(TowerDetail detail) => _towerDetail = detail;

    private void OnEnableUpgradeBtn()
    {
        if(_typeBtn != OptionTowerType.Upgrade) return;
        _towerDetail = _createTower.CurrentTowerDetail;
        if(_towerDetail.GetNextTowerInfo(_createTower.TowerLevel) == null)
        {
            _btn.interactable = false;
            _imgSlot.sprite = _lockSprite;
            _price = 0;
            _priceTag.transform.parent.gameObject.SetActive(false);
            return;
        }
        InteractableBtn();
    }
    private void OnEnableSellBtn()
    {
        if(_typeBtn != OptionTowerType.Sell) return;
        _towerDetail = _createTower.CurrentTowerDetail;
        _imgSlot.sprite = _mainSprite;
        _price = _towerDetail.GetTowerInfo(_createTower.TowerLevel).SellMoney;
        _priceTag.text = _price.ToString();
    }
    private void OnEnablePurchaseTowerBtn()
    {
        if(_typeBtn != OptionTowerType.PurchaseTower) return;
        InteractableBtn();
    }

    private void OnEnableOrderMovingBtn()
    {
        if(_typeBtn != OptionTowerType.OrderMoving) return;
        _towerDetail = _createTower.CurrentTowerDetail;
        _imgSlot.sprite = _mainSprite;
    }

    private void InteractableBtn()
    {
        _btn.interactable = true;
        _imgSlot.sprite = _mainSprite;
        _price = _towerDetail.GetNextTowerInfo(_createTower.TowerLevel).Cost;
        _priceTag.text = _price.ToString();
        _priceTag.transform.parent.gameObject.SetActive(true);
        if(GameController.Instance.ResourceManagement.TotalMoney < _price)
        {
            _btn.interactable = false;
            _imgSlot.sprite = _unableSprite;
            _priceTag.color = Color.gray;
        }
    }
}
