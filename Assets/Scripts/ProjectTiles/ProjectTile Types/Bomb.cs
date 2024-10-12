using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : ProjectTiles
{
    [SerializeField]
    private Animator _animator;
    [SerializeField] private float _explosionRadius;
    [SerializeField]
    private AnimationCurve _pathCurve;
    private bool _falling = false;
    [SerializeField]
    private float _heightY;
    [SerializeField]
    private Vector3 start;
    [SerializeField]
    private Vector2 target;
    [SerializeField] private float _duration;
    protected override void Start()
    {
        _animator = GetComponent<Animator>();
        start = transform.position;
    }

    protected override void MoveProjectTiles()
    {
        
    }
    protected override void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartLaunch(start, target);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            transform.position = start;
            
        }
    }
    public void StartLaunch(Vector3 start, Vector2 target)
    {
        StartCoroutine(CurvePath(start, target, _duration, _heightY, _pathCurve));   
    }
    
    protected override void RotateProjectTiles()
    {
        transform.rotation = Quaternion.Euler(0,0,50f);
    }
}
