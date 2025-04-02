using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class SoldierTower : Tower, IOrderable
{
    [Header("References")]
    [SerializeField]
    private GameObject _soldierPrefab;
    [SerializeField]
    private Soldier[] _soldierList;
    [Header("Attributes")]
    [SerializeField]
    private int _soldiersCapacity = 1;

    [SerializeField] 
    private Vector2 _rallyPosition = new Vector3(0,-2f,0);
    private Vector2[] _soldierPosArr;
    [SerializeField]
    private float _rallyRadius = 1.5f;
    private int _soldiersCount = 0;
    private bool _spawnOrder = false;
    public bool HaveOrder { get; private set; } = false;

    Vector2 CalSoldierPos(float angle)
        => new Vector2(
            x: Mathf.Cos(angle) * _rallyRadius,
            y: Mathf.Sin(angle) * _rallyRadius);

    public override void Initialise()
    {
        _soldierPosArr = new Vector2[_soldiersCapacity];
        _soldierList = new Soldier[_soldiersCapacity];
        for (int i = 0; i < _soldiersCapacity; i++)
        {
            if(PoolingObject.Instance.GetObject(_soldierPrefab).TryGetComponent<Soldier>(out Soldier newSoldier))
            {
                var angle = i*Mathf.PI *2/_soldiersCapacity;
                _soldierPosArr[i] = CalSoldierPos(angle);
                newSoldier.transform.position = transform.position;
                newSoldier.transform.SetParent(transform);
                _soldierList[i] = newSoldier;
                newSoldier.gameObject.SetActive(true);
            }
        }
        _soldiersCount = _soldiersCapacity;
        HaveOrder = true;
        _spawnOrder = true;
    }
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        OrderAction();
    }

    protected override void UpdateCurrentTarget()
    {
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
    }

    public void OrderAction()
    {
        if(!HaveOrder) return;
        if(Input.GetMouseButtonUp(0))
        {
            Vector2 newPos = GameController.Instance.GetMousePosition();
            if(Vector2.Distance(newPos, transform.position) <= GetColliderRange()*transform.localScale.x)
            {
                _rallyPosition = newPos;
                for(int i = 0; i < _soldiersCapacity; i++)
                {
                    if(_soldierList[i].GetIsDead())  continue;
                    _soldierList[i].MovingTo(_soldierPosArr[i] + _rallyPosition);
                }
            }
            SetOrder(false);
            _spawnOrder = false;
        }
        else if(_spawnOrder)
        {
            for(int i = 0; i < _soldiersCapacity; i++)
                {
                    if(_soldierList[i].GetIsDead())  continue;
                    _soldierList[i].MovingTo(_soldierPosArr[i] + _rallyPosition);
                }
            SetOrder(false);
            _spawnOrder = false;
        }
        
    }

    public void SetOrder(bool order)
    {
        HaveOrder = order;
    }
}
