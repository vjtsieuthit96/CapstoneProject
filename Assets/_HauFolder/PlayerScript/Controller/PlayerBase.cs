using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    // data
    public PlayerData Data;
    [SerializeField] private GunController Gun;
    
    // interfaces
    private ILookBehavior PlayerLook;
    private PlayerStateManager StateManager;
    private IInputSystem Input;

    // References
    private PlayerCore Core;

    // base component
    private Rigidbody rb;
    private Transform PlayerTransform;
    private GroundCheck groundCheck;
    public Transform cameraTransform;
    private CameraRecoil CamRecoil;
    // Coordinates
    private float RotatonX;
    private float RotatonY;

    private void Awake()
    {
        CamRecoil = cameraTransform.GetComponent<CameraRecoil>();
        PlayerTransform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        PlayerLook = new PlayerLook();
        groundCheck = GetComponent<GroundCheck>();
        Input = new TestInputController();
        CameraBobbing camBobbing = cameraTransform.GetComponent<CameraBobbing>();
        StateManager = new PlayerStateManager(groundCheck, Input, rb, Data, camBobbing);
        Core = new PlayerCore(Input, StateManager, PlayerLook, rb, Data);
        Core.Equip(Gun);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        StateManager.UpdateState();
        PlayerMovement();
        PlayerLookable();
        PlayerShoot();
        PlayerReload();
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
        Core.PlayerLook(lookvec, ref RotatonX, ref RotatonY, PlayerTransform, cameraTransform);
    }
    private void PlayerShoot()
    {
        Core.ManualUpdate();
        if (Input.IsPress())
        {
            Core.OnShoot();
            if(Gun.CurrentAmmo > 0)
            {
                Core.GunEffect(Gun.Data, this.gameObject.transform, CamRecoil, ref RotatonX, ref RotatonY);
            }
        }
        if (Input.IsRelease())
        {
            Core.OffShoot();
        }
    }
    private void PlayerReload()
    {
        if (Input.IsReload())
        {
            Core.Reload();
        }
    }
}

