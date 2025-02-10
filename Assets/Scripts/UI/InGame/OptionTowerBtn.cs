using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionTowerBtn : MonoBehaviour
{
    private int _price;
    private CreateTower _createTower;
    [SerializeField]
    private Sprite _mainSprite;
    [SerializeField]
    private Sprite _unableSprite;
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
        _createTower = transform.root.GetComponent<CreateTower>();
        _imgSlot = GetComponent<Image>();
        _priceTag = GetComponentInChildren<Text>();
        _btn = GetComponent<Button>();
        _btn.onClick.AddListener(OnClick);
        _type = TowerType.Mage;
        _imgSlot.sprite = _mainSprite;
        _price = 100;
    }
    private void Start() {
        
    }
    private void OnEnable() {
        _isSelected = false;
        if(GameController.Instance.ResourceManagement.TotalMoney >= _price)
        {
            _imgSlot.sprite = _mainSprite;
            
            return;
        }
        _imgSlot.sprite = _unableSprite;
        _priceTag.text = _price.ToString();
        _priceTag.color = Color.gray;
    }
    public void OnClick()
    {
        if(!_isSelected)
        {
            _isSelected = true;
            _imgSlot.sprite = _selectedSprite;
            return;
        }
        _isSelected = false;
        
        _createTower.ChooseTower(_type);
    }
    public void SetType(TowerType type) => _type = type;
}
