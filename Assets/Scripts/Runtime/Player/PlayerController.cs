using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player Controller that handles weapon inputs, movement inputs and look inputs. Uses the Weapon, Controller3D script.
/// </summary>

[RequireComponent(typeof(Controller3D))]
[RequireComponent(typeof(Weapon))]
[RequireComponent(typeof(TeleportHandler))]
[RequireComponent(typeof(HealHandler))]
[RequireComponent(typeof(AimHandler))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Vector2 lookAngle;
    private Controller3D controller;
    private Weapon weapon;
    private TeleportHandler teleportHandler;
    private HealHandler healHandler;
    private AimHandler aimHandler;
    private PlayerInputs playerInputs;
    private InputAction move;
    private InputAction sprint;
    private InputAction look;
    private InputAction attack;
    private InputAction aim;
    private InputAction reload;
    private InputAction teleport;
    private InputAction heal;
    void Awake()
    {
        controller = GetComponent<Controller3D>();
        weapon = GetComponent<Weapon>();
        teleportHandler = GetComponent<TeleportHandler>();
        healHandler = GetComponent<HealHandler>();
        aimHandler = GetComponent<AimHandler>();
        playerInputs = new PlayerInputs();
    }
    private void OnEnable()
    {
        move = playerInputs.Player.Move;
        move.Enable();
        sprint = playerInputs.Player.Sprint;
        sprint.Enable();
        look = playerInputs.Player.Look;
        look.Enable();
        attack = playerInputs.Player.Attack;
        attack.Enable();
        aim = playerInputs.Player.Aim;
        aim.Enable();
        reload = playerInputs.Player.Reload;
        reload.Enable();
        teleport = playerInputs.Player.Teleport;
        teleport.Enable();
        heal = playerInputs.Player.Heal;
        heal.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        sprint.Disable();
        look.Disable();
        attack.Disable();
        aim.Disable();
        reload.Disable();
        teleport.Disable();
        heal.Disable();
    }
    void Update()
    {
        if (attack.IsPressed())
        {
            weapon.Shoot();
        }
        if (reload.WasPressedThisFrame())
        {
            weapon.Reload();
        }
        if (teleport.WasPressedThisFrame())
        {
            teleportHandler.AttemptTeleport();
        }
        if (heal.WasPressedThisFrame())
        {
            healHandler.AttemptHeal();
        }
        aimHandler.HandleAim(aim.IsPressed());
        controller.isSprinting = sprint.IsPressed();
        controller.HandleHorizontalLook(look.ReadValue<Vector2>().x, lookSensitivity);
        cameraController.HandleVerticalLook(-look.ReadValue<Vector2>().y, lookSensitivity, lookAngle);
    }
    private void FixedUpdate()
    {
        controller.HandleMove(move.ReadValue<Vector2>());
    }
}
