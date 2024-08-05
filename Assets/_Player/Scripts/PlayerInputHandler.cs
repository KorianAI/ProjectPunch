using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement pm;
    private PlayerCombat pc;
    private PlayerStateManager sm;

    //public InputMaster InputMaster { get; private set; }

    private Stack<InputCommand> inputBuffer = new Stack<InputCommand>();
    public float bufferTime = 0.5f; // Buffer time in seconds
    public float bufferTimer;

    public bool canConsumeInput;

    public Vector2 inputDir;

    private void Awake()
    {
        pm = GetComponent<PlayerMovement>();
        pc = GetComponent<PlayerCombat>();
        sm = GetComponent<PlayerStateManager>();
        //InputMaster = new InputMaster();
    }

    //void OnEnable()
    //{
    //    InputMapManager.inputActions.Enable();
    //}

    //void OnDisable()
    //{
    //    InputMapManager.inputActions.Disable();
    //}
    
    private void Start()
    {
        InputMapManager.inputActions.Player.LightAttack.started += ctx =>
        {
            BufferInput(new InputCommand { Type = InputType.X, Direction = InputMapManager.inputActions.Player.Movement.ReadValue<Vector2>() });
        };

        InputMapManager.inputActions.Player.LightAttackHold.performed += ctx =>
        {
            BufferInput(new InputCommand { Type = InputType.xH, Direction = InputMapManager.inputActions.Player.Movement.ReadValue<Vector2>() });
        };

        InputMapManager.inputActions.Player.HeavyAttack.started += ctx =>
        {
            BufferInput(new InputCommand { Type = InputType.Y, Direction = InputMapManager.inputActions.Player.Movement.ReadValue<Vector2>() });
        };

        InputMapManager.inputActions.Player.Jump.started += ctx =>
        {
            BufferInput(new InputCommand { Type = InputType.A });
        };

        InputMapManager.inputActions.Player.Dash.started += ctx =>
        {
            BufferInput(new InputCommand { Type = InputType.B });
        };

        InputMapManager.inputActions.Player.Push.started += ctx =>
        {
            BufferInput(new InputCommand { Type = InputType.Push, Direction = InputMapManager.inputActions.Player.Movement.ReadValue<Vector2>() });
        };

        InputMapManager.inputActions.Player.Pull.started += ctx =>
        {
            BufferInput(new InputCommand { Type = InputType.Pull, Direction = InputMapManager.inputActions.Player.Movement.ReadValue<Vector2>() });
        };
    }

    private void Update()
    {
        bufferTimer -= Time.deltaTime;
        if (bufferTimer <= 0)
        {
            inputBuffer.Clear(); // Clear buffer after timer expires
        }

        if (canConsumeInput)
        {
            ConsumeBufferedInput();
        }
    }

    private void BufferInput(InputCommand command)
    {
        inputBuffer.Push(command); // Add to the stack
        bufferTimer = bufferTime; // Reset buffer timer when a new command is added
    }

    public void ConsumeBufferedInput()
    {
        if (inputBuffer.Count > 0)
        {
            sm.currentState.HandleBufferedInput(inputBuffer.Pop()); // Pass the command to the current state for handling
            
            canConsumeInput = false;
        }
    }

    public void SetCanConsumeInput(bool canConsume)
    {
        canConsumeInput = canConsume;
    }

    public InputCommand[] GetBufferedInputs()
    {
        return inputBuffer.ToArray();
    }

    public List<InputCommand> GetBufferedInputsForInspector()
    {
        return new List<InputCommand>(inputBuffer);
    }
}
