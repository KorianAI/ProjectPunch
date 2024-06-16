using UnityEngine;

public abstract class PlayerState
{
    public abstract void EnterState(PlayerStateManager player);
    public abstract void ExitState(PlayerStateManager player) ;
    public abstract void FrameUpdate(PlayerStateManager player); // regular update
    public abstract void PhysicsUpdate(PlayerStateManager player); // fixed update

    public abstract void HandleBufferedInput(InputCommand command);
}
