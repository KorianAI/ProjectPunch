using UnityEngine;

public abstract class PlayerState
{
    public abstract void EnterState(PlayerStateManager player);
    public abstract void ExitState(PlayerStateManager player) ;
    public abstract void FrameUpdate(PlayerStateManager player);
    public abstract void PhysicsUpdate(PlayerStateManager player);
}
