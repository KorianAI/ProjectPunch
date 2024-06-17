using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerIdleState : PlayerState
{
    public override void EnterState(PlayerStateManager player)
    {
        
        _sm = player;
        player.pm.velocity = Vector3.zero;
        _sm.inputHandler.SetCanConsumeInput(true);
    }

    public override void ExitState(PlayerStateManager player)
    {
        
    }

    public override void FrameUpdate(PlayerStateManager player)
    {

        if (!player.pm.grounded)
        {
            player.SwitchState(player.inAirState);
        }

        if (player.inputHandler.InputMaster.Player.Movement.ReadValue<Vector2>() != Vector2.zero)
        {
            player.SwitchState(player.moveState);
        }
    }

    public override void PhysicsUpdate(PlayerStateManager player)
    {
        
    }

    public override void HandleBufferedInput(InputCommand command)
    {
        if (command.Type == InputType.X)
        {
            Debug.Log("Light Attack received in move state");
            // Example transition to Attack1State
            _sm.SwitchState(new PlayerLightAttack());
        }

        else if (command.Type == InputType.Y)
        {
            Debug.Log("Heavy Attack received in move state");
            _sm.SwitchState(new PlayerHeavyAttack());
        }
    }
}
