using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

[RequireComponent(typeof(Weapon))]
public class WeaponSwappingHandler : MonoBehaviour
{
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private Transform weaponLocation;
    [SerializeField] private WeaponData weapon1;
    [SerializeField] private WeaponData weapon2;
    [SerializeField] private WeaponData weapon3;
    [SerializeField] private bool weapon2Unlocked = false;
    [SerializeField] private bool weapon3Unlocked = false;
    private Weapon weapon;
    private GameObject weaponObject;
    private PlayerInputs playerInputs;
    private InputAction swapWeapon1;
    private InputAction swapWeapon2;
    private InputAction swapWeapon3;

    void Awake()
    {
        playerInputs = new PlayerInputs();
        weapon = GetComponent<Weapon>();
        StartCoroutine(HandleSwapWeapon(weapon1));
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
            StartCoroutine(HandleSwapWeapon(weapon1));
        }
        if (swapWeapon2.WasPressedThisFrame() && weapon2Unlocked)
        {
            StartCoroutine(HandleSwapWeapon(weapon2));
        }
        if (swapWeapon3.WasPressedThisFrame() && weapon3Unlocked)
        {
            StartCoroutine(HandleSwapWeapon(weapon3));
        }
    }

    private IEnumerator HandleSwapWeapon(WeaponData newWeapon)
    {
        if(weapon.weaponData == newWeapon)
        {
            yield break;
        }
        weapon.canShoot = false;
        weaponAnimator.SetTrigger("WeaponSwap");
        yield return new WaitForSeconds(0.25f);
        weapon.SwapWeapon(newWeapon);
        Destroy(weaponObject);
        weaponObject = Instantiate(newWeapon.weaponObject, weaponLocation);
        weaponObject.transform.SetParent(weaponLocation);
        yield return new WaitForSeconds(0.25f);
        weapon.canShoot = true;
    }

    public void UnlockWeapon(EnumLibrary.EWeaponPickup weaponPickup)
    {
        switch (weaponPickup)
        {
            case EnumLibrary.EWeaponPickup.weapon2:
                weapon2Unlocked = true;
                break;
            case EnumLibrary.EWeaponPickup.weapon3:
                weapon3Unlocked = true;
                break;
        }
    }
}
