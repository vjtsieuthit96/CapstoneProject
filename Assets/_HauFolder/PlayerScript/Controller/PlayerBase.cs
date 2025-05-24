using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    // data
    public PlayerData Data;

    // interfaces
    private ILookBehavior PlayerLook;
    private PlayerStateManager StateManager;
    private IInputSystem Input;

    // References
    private PlayerCore Core;

    // base component
    private Rigidbody rb;
    private Transform PlayerTransform;
    public Transform CameraTransform;
    private GroundCheck groundCheck;
    public Transform cameraTransform;

    // Coordinates
    private float RotatonX;
    private float RotatonY;

    private void Awake()
    {
        PlayerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        PlayerLook = new PlayerLook();
        groundCheck = GetComponent<GroundCheck>();
        Input = new TestInputController();
        CameraBobbing camBobbing = cameraTransform.GetComponent<CameraBobbing>();
        StateManager = new PlayerStateManager(groundCheck, Input, rb, Data, camBobbing);
        Core = new PlayerCore(Input, StateManager, PlayerLook, rb, Data);
    }

    private void Update()
    {
        StateManager.UpdateState();
        PlayerMovement();
        PlayerLookable();
    }

    private void PlayerMovement()
    {
        Vector2 inputvec = Input.MoveInput();
        Vector3 Diretion = new Vector3(inputvec.x, 0, inputvec.y);
        Diretion = transform.TransformDirection(Diretion);
        Core.PlayerMove(Diretion);
    }
    private void PlayerLookable()
    {
        Vector2 lookvec = Input.LookInput();
        Core.PlayerLook(lookvec, ref RotatonX, ref RotatonY, PlayerTransform, CameraTransform);
    }


}
