using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DetMonobehaviour : MonoBehaviour
{
    public virtual void Initialise(){}
    protected virtual void LoadComponents(){}

    protected virtual void Awake()
    {
        LoadComponents();
    }

    protected virtual void Reset()
    {
        LoadComponents();
    }
}
