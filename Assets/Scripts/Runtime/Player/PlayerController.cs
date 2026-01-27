using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

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
    [SerializeField] private GameObject menuObject;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Vector2 lookAngle;
    private bool menuIsOpen;
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
    private InputAction menu;
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
        menu = playerInputs.Player.Menu;
        menu.Enable();
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
        menu.Disable();
    }
    void Update()
    {
        if (attack.IsPressed() && !menuIsOpen)
        {
            weapon.Shoot();
        }
        if (reload.WasPressedThisFrame() && !menuIsOpen)
        {
            weapon.Reload();
        }
        if (teleport.WasPressedThisFrame() && !menuIsOpen)
        {
            teleportHandler.AttemptTeleport();
        }
        if (heal.WasPressedThisFrame() && !menuIsOpen)
        {
            healHandler.AttemptHeal();
        }
        if (menu.WasPressedThisFrame())
        {
            OpenCloseMenu();
        }
        aimHandler.HandleAim(aim.IsPressed());
        controller.isSprinting = sprint.IsPressed();
        controller.HandleHorizontalLook(look.ReadValue<Vector2>().x, lookSensitivity * (menuIsOpen ? 0 : 1));
        cameraController.HandleVerticalLook(-look.ReadValue<Vector2>().y, lookSensitivity * (menuIsOpen ? 0 : 1), lookAngle);
    }
    private void FixedUpdate()
    {
        controller.HandleMove(move.ReadValue<Vector2>());
    }

    private void OpenCloseMenu()
    {
        if (menuIsOpen)
        {
            menuIsOpen = false;
            menuObject.SetActive(false);
            controller.enabled = true;
            aimHandler.enabled = true;
            Time.timeScale = 1;
            CursorManager.Instance.ChangeMouseLock(false, true, false);
        }
        else
        {
            menuIsOpen = true;
            menuObject.SetActive(true);
            controller.enabled = false;
            aimHandler.enabled = false;
            Time.timeScale = 0;
            CursorManager.Instance.ChangeMouseLock(true, false, false);
        }
    }
}
