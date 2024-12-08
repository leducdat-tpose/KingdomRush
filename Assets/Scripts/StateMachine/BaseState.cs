using UnityEngine;

public abstract class BaseState<T>
{
    protected readonly T Object;
    protected StateManager<T> ObjectStateManager;

    protected BaseState(T obj, StateManager<T> objectStateManager)
    {
        Object = obj;
        ObjectStateManager = objectStateManager;
    }
    
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void FrameUpdate();
    public abstract void PhysicsUpdate();
    public virtual void AnimationTriggerEvent() { }
    public virtual void GetNextState() { }
    public virtual void OnTriggerEnter(Collider2D collision) { }
    public virtual void OnTriggerStay(Collider2D collision) { }
    public virtual void OnTriggerExit(Collider2D collision) { }
}
