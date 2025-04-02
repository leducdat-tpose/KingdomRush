using UnityEngine;

public abstract class BaseState<T>
{
    protected readonly T owner;
    protected StateManager<T> stateManager;

    protected BaseState(T owner, StateManager<T> objectStateManager)
    {
        this.owner = owner;
        stateManager = objectStateManager;
    }
    
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void Update();
    public abstract void FixedUpdate();
    public virtual void AnimationTriggerEvent() { }
    public virtual void GetNextState() { }
    public virtual void OnTriggerEnter2D(Collider2D other) { }
    public virtual void OnTriggerStay2D(Collider2D other) { }
    public virtual void OnTriggerExit2D(Collider2D other) { }
}
