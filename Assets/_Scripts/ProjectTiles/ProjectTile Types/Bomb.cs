using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ProjectTiles
{
    [SerializeField] private float _explosionRadius;
    [SerializeField]
    private AnimationCurve _pathCurve;
    [SerializeField]
    private float _heightY;
    [SerializeField] private float _duration;
    protected override void Start()
    {
        base.Start();
        StartCoroutine(CurvePath(transform.position, EnemyTarget.gameObject.transform.position, _duration, _heightY, _pathCurve));
    }

    protected override void MoveProjectTiles()
    {
        
    }
    protected override void Update()
    {
        RotateProjectTiles();
    }
    public void StartLaunch(Vector3 start, Vector2 target)
    {
        StartCoroutine(CurvePath(start, target, _duration, _heightY, _pathCurve));   
    }
    
    protected override void RotateProjectTiles()
    {
        transform.Rotate(0f, 0f, 180f * Time.deltaTime);
    }
}
