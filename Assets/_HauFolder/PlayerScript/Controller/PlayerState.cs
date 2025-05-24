using UnityEngine;

public abstract class PlayerState
{
    public IMovementBehavior MoveBehavior;
    public PlayerData data;
    public abstract void HandleMovement(Rigidbody rb, Vector3 Direction, PlayerData data);
}

public class PlayerNormalMove : PlayerState
{
    public PlayerNormalMove()
    {
        MoveBehavior = new PlayerMovement();
    }

    public override void HandleMovement(Rigidbody rb, Vector3 Direction, PlayerData data)
    {
        MoveBehavior.Move(rb, Direction, data);
    }
}

public class PlayerSprintMove : PlayerState
{
    public PlayerSprintMove()
    {
        MoveBehavior = new PlayerSprintMovement();
    }

    public override void HandleMovement(Rigidbody rb, Vector3 Direction, PlayerData data)
    {
        MoveBehavior.Move(rb, Direction, data);
    }
}

public class PlayerStateManager
{
    private PlayerState state;
    private GroundCheck checker;
    private PlayerData data;
    private Rigidbody rb;
    private IInputSystem input;

    public PlayerStateManager (GroundCheck checker, IInputSystem input, Rigidbody rb, PlayerData data)
    {
        this.checker = checker;
        this.rb = rb;
        state = checker.IsGrounded ? new PlayerNormalMove() : new PlayerSprintMove();
        this.input = input;
        this.data = data;
    }

    public void UpdateState()
    {
        if(!input.IsSpringting())
        {
            state = new PlayerNormalMove();
        }
        else
        {
            state = new PlayerSprintMove();
        }
    }

    public void PlayerHandleMovement(Vector3 Direction)
    {
        state.HandleMovement(rb, Direction, data);
    }
}