using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Player Controller that handles weapon inputs, movement inputs and look inputs. Uses the Weapon, Controller3D script.
/// </summary>

[RequireComponent(typeof(Controller3D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private Vector2 lookAngle;
    private Controller3D controller;
    private PlayerInputs playerInputs;
    private InputAction move;
    private InputAction look;
    private InputAction attack;
    private InputAction reload;
    void Awake()
    {
        controller = GetComponent<Controller3D>();
        playerInputs = new PlayerInputs();
    }
    private void OnEnable()
    {
        move = playerInputs.Player.Move;
        move.Enable();
        look = playerInputs.Player.Look;
        look.Enable();
        attack = playerInputs.Player.Attack;
        attack.Enable();
        reload = playerInputs.Player.Reload;
        reload.Enable();
    }
    private void OnDisable()
    {
        move.Disable();
        look.Disable();
        attack.Disable();
        reload.Disable();
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
        controller.HandleHorizontalLook(look.ReadValue<Vector2>().x, lookSensitivity);
        cameraController.HandleVerticalLook(-look.ReadValue<Vector2>().y, lookSensitivity, lookAngle);
    }
    private void FixedUpdate()
    {
        controller.HandleMove(move.ReadValue<Vector2>());
    }
}
