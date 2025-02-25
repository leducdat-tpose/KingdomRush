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
    private List<Soldier> _soldierList;
    [Header("Attributes")]
    [SerializeField]
    private int _soldiersCapacity = 1;

    [SerializeField] 
    private Vector2 _gatherPosition = new Vector3(-4,0,0);
    [SerializeField]
    private float _gatherPosRadius = 1.5f;
    [SerializeField]
    private int _soldiersCount;
    public bool HaveOrder { get; private set; } = false;

    public override void Initialise()
    {
        for (int i = 0; i < _soldiersCapacity; i++)
        {
            Soldier newSoldier = PoolingObject.Instance.GetObject(_soldierPrefab).GetComponent<Soldier>();
            newSoldier.transform.position = _gatherPosition
            + new Vector2(Random.Range(-_gatherPosRadius, _gatherPosRadius),Random.Range(-_gatherPosRadius, _gatherPosRadius));
            newSoldier.transform.SetParent(transform);
            _soldierList.Add(newSoldier);
            newSoldier.gameObject.SetActive(true);
        }
        _soldiersCount = _soldierList.Count;
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
        if(!Input.GetMouseButtonUp(0)) return;
        var newPos = GameController.Instance.GetMousePosition();
        if(Vector2.Distance(newPos, transform.position) <= GetColliderRange()*transform.localScale.x)
        {
            foreach(var soldier in _soldierList)
            {
                if(soldier.Behaviour.GetIsDead()) continue;
                soldier.Behaviour.MovingTo(newPos);
            }
        }
        SetOrder(false);
    }

    public void SetOrder(bool order)
    {
        HaveOrder = order;
    }
}
