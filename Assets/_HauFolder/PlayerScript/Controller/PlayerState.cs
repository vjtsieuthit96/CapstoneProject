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
    private CameraBobbing cameraBobbing;

    public PlayerStateManager (GroundCheck checker, IInputSystem input, Rigidbody rb, PlayerData data, CameraBobbing cameraBobbing)
    {
        this.checker = checker;
        this.rb = rb;
        state = checker.IsGrounded ? new PlayerNormalMove() : new PlayerSprintMove();
        this.input = input;
        this.data = data;
        this.cameraBobbing = cameraBobbing;
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
        ApplyBobbing(Direction);
    }
    private void ApplyBobbing(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.01f)
        {
            cameraBobbing.SetBobbing(0f, 0f);
            return;
        }

        float speed = input.IsSpringting()
            ? data.PlayerDefaultSpeed * data.RatioRun
            : data.PlayerDefaultSpeed;

        float weightFactor = Mathf.Clamp(data.PlayerWeight, 0.5f, 2f);
        float amplitude = 0.03f * speed * weightFactor;
        float frequency = 3.5f * speed * (1f / weightFactor);
        amplitude = Mathf.Clamp(amplitude, 0.01f, 0.1f);
        frequency = Mathf.Clamp(frequency, 1f, 10f);
        cameraBobbing.SetBobbing(amplitude, frequency);
    }
}