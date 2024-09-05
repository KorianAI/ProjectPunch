using UnityEngine;

public class PlayerState
{
    public PlayerStateManager _sm;
    protected float time { get; set; }
    protected float fixedtime { get; set; }

    public virtual void EnterState(PlayerStateManager player)
    {
        _sm = player;
        //Debug.Log("Into" + this.GetType().Name);
    }

    public virtual void ExitState(PlayerStateManager player)
    {
        //Debug.Log("Leaving" + this.GetType().Name);
    }

    public virtual void FrameUpdate(PlayerStateManager player)
    {
        time += Time.deltaTime;
        
    }
    public virtual void PhysicsUpdate(PlayerStateManager player) // fixed update
    {
        fixedtime += Time.deltaTime;
        
    }

    public virtual void HandleBufferedInput(InputCommand command)
    {
        if (command == null) return;

        if (command.Type == InputType.X || command.Type == InputType.xH)
        {
            _sm.resources.attachment.WeaponInput(command, _sm.pm.grounded, _sm.pc.attackIndex);
        }

        else if (command.Type == InputType.Y)
        {
            _sm.resources.mode.WeaponInput(command, _sm.pm.grounded, _sm.pc.attackIndex);
        }

        else if (command.Type == InputType.A)
        {
            _sm.pm.Jump();
        }

        else if (command.Type == InputType.B)
        {
            if (_sm.pm.grounded)
            {
                _sm.SwitchState(new PlayerDashState());
            }

            else
            {
                if (_sm.pm.airDashAmount < _sm.pm.maxAirDash)
                {
                    _sm.SwitchState(new PlayerDashState());
                    _sm.pm.airDashAmount++;
                }
            }               
        }

        else if (command.Type == InputType.Push)
        {

                _sm.magnets.PushInput(command, _sm.pm.grounded);

        }

        else if (command.Type == InputType.Pull)
        {
            _sm.magnets.PullInput(command);
        }

        _sm.ih.inputDir = command.Direction;
    }


    #region Passthrough Methods

    /// <summary>
    /// Removes a gameobject, component, or asset.
    /// </summary>
    /// <param name="obj">The type of Component to retrieve.</param>
    protected static void Destroy(UnityEngine.Object obj)
    {
        UnityEngine.Object.Destroy(obj);
    }

    /// <summary>
    /// Returns the component of type T if the game object has one attached, null if it doesn't.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected T GetComponent<T>() where T : Component { return _sm.GetComponent<T>(); }

    /// <summary>
    /// Returns the component of Type <paramref name="type"/> if the game object has one attached, null if it doesn't.
    /// </summary>
    /// <param name="type">The type of Component to retrieve.</param>
    /// <returns></returns>
    protected Component GetComponent(System.Type type) { return _sm.GetComponent(type); }

    /// <summary>
    /// Returns the component with name <paramref name="type"/> if the game object has one attached, null if it doesn't.
    /// </summary>
    /// <param name="type">The type of Component to retrieve.</param>
    /// <returns></returns>
    protected Component GetComponent(string type) { return _sm.GetComponent(type); }
    #endregion
}
