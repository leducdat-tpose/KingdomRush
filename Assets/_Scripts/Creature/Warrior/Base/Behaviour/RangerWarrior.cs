using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RangerWarrior : Warrior
{
    private float _nextAttackTime;
    private Enemy _currentTargetEnemy;
    [SerializeField]
    public Tower MainTower{get; private set;}
    private Vector2 PositionReleaseProjectTile;
    private ProjectTiles _currentProjectTile;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        MainTower = transform.root.GetComponentInChildren<Tower>();
    }
    private void Start() {
        Initialise();
    }
    public override void Initialise()
    {
        base.Initialise();
        _nextAttackTime = 0;
        _currentTargetEnemy = null;
        PositionReleaseProjectTile = transform.position;
        currentAnimation = idleAnimation;
    }

    public override void Render()
    {
        int newAnimation = idleAnimation;
        if (stateManager.CurrentState != idleState)
        {
            if (stateManager.CurrentState == attackState)
                newAnimation = attackAnimation;
            else if (stateManager.CurrentState == deathState)
            {
                newAnimation = deathAnimation;
            }
            else if (stateManager.CurrentState == walkState)
            {
                newAnimation = walkSideAnimation;
            }
            if(MainTower.CurrentTarget)
            {
                Vector2 direction = MainTower.CurrentTarget.transform.position - MainTower.transform.position;
                spriteRenderer.flipX = direction.x < -0.8f;
                if(direction.y > 1.0f && stateManager.CurrentState == attackState) newAnimation = attackUpAnimation;
            }
        }
        if(currentAnimation == newAnimation) return;
        animator.CrossFade(newAnimation, 0.1f, 0);
        currentAnimation = newAnimation;
    }

    public override void Update()
    {
        UpdateCurrentTargetEnemy();
        ReadyToAttack();
        stateManager.Update();
        Render();
    }

    public override void FixedUpdate()
    {
        stateManager.FixedUpdate();
    }

    public override void ReadyToAttack()
    {
        if (!MainTower.CurrentTarget 
        || MainTower.CurrentTarget.GetIsDead() 
        || Time.time < _nextAttackTime
        || stateManager.CurrentState == attackState) return;
        stateManager.ChangeState(attackState);
        _nextAttackTime = Time.time + data.CoolDownAttack;
    }

    public override void StartAttacking()
    {
        //Execute the tower's animate
        MainTower.StartProgress();
        ReadyProjectile();
    }
    public override void StopAttacking()
    {
        stateManager.ChangeState(idleState);
    }

    public void ShootProjectTile(){
        _currentProjectTile.gameObject.SetActive(true);
    }

    public override void ReadyProjectile()
    {
        // var newObject = PoolingObject.Instance.GetObject(Object.PrefabProjectTile);
        var newObject = PoolingObject.Instance.GetObject(data.ProjectilePrefab);
        if(newObject.TryGetComponent(out ProjectTiles projectTiles))
        {
            _currentProjectTile = projectTiles;
            _currentProjectTile.transform.position = PositionReleaseProjectTile;
            _currentProjectTile.gameObject.SetActive(false);
            _currentProjectTile.SetCurrentEnemy(_currentTargetEnemy);
            _currentProjectTile.SetCauseDamage(data.Damage);
        }
        else Debug.Log("Can't get ProjectTiles");
    }
    public void UpdateCurrentTargetEnemy()
    {
        if(_currentTargetEnemy == MainTower.CurrentTarget) return;
        _currentTargetEnemy = MainTower.CurrentTarget;
    }
}
