using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[RequireComponent(typeof(Weapon))]
public class WeaponSwappingHandler : MonoBehaviour
{
    [SerializeField] private WeaponData weapon1;
    [SerializeField] private WeaponData weapon2;
    [SerializeField] private WeaponData weapon3;
    private Weapon weapon;
    private PlayerInputs playerInputs;
    private InputAction swapWeapon1;
    private InputAction swapWeapon2;
    private InputAction swapWeapon3;

    void Awake()
    {
        playerInputs = new PlayerInputs();
        weapon = GetComponent<Weapon>();
    }
    private void OnEnable()
    {
        swapWeapon1 = playerInputs.Player.SwapWeapon1;
        swapWeapon1.Enable();
        swapWeapon2 = playerInputs.Player.SwapWeapon2;
        swapWeapon2.Enable();
        swapWeapon3 = playerInputs.Player.SwapWeapon3;
        swapWeapon3.Enable();
    }

    private void OnDisable()
    {
        swapWeapon1.Disable();
        swapWeapon2.Disable();
        swapWeapon3.Disable();
    }

    private void Update()
    {
        if (swapWeapon1.WasPressedThisFrame())
        {
            weapon.SwapWeapon(weapon1);
        }
        if (swapWeapon2.WasPressedThisFrame())
        {
            weapon.SwapWeapon(weapon2);
        }
        if (swapWeapon3.WasPressedThisFrame())
        {
            weapon.SwapWeapon(weapon3);
        }
    }
}
