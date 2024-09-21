using UnityEngine;

public class BaseState<T>
{
    protected readonly T Object;
    protected StateManager<T> ObjectStateManager;

    protected BaseState(T obj, StateManager<T> objectStateManager)
    {
        Object = obj;
        ObjectStateManager = objectStateManager;
    }
    
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void FrameUpdate() { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent() { }
    public virtual void GetNextState() { }
    public virtual void OnTriggerEnter(Collider2D collision) { }
    public virtual void OnTriggerStay(Collider2D collision) { }
    public virtual void OnTriggerExit(Collider2D collision) { }
}
