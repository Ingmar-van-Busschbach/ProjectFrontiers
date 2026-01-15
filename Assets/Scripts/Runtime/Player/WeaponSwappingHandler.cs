using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Weapon))]
public class WeaponSwappingHandler : MonoBehaviour
{
    [SerializeField] private Animator weaponAnimator;
    [SerializeField] private Transform weaponLocation;
    [SerializeField] private List<WeaponData> weapons = new List<WeaponData>();
    [SerializeField] private int weaponsUnlocked = 1;
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
        StartCoroutine(HandleSwapWeapon(weapons[0]));
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
        if (swapWeapon1.WasPressedThisFrame() && weaponsUnlocked >= 1)
        {
            StartCoroutine(HandleSwapWeapon(weapons[0]));
        }
        if (swapWeapon2.WasPressedThisFrame() && weaponsUnlocked >= 2)
        {
            StartCoroutine(HandleSwapWeapon(weapons[1]));
        }
        if (swapWeapon3.WasPressedThisFrame() && weaponsUnlocked >= 3)
        {
            StartCoroutine(HandleSwapWeapon(weapons[2]));
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

    public void UnlockWeapon(WeaponData weaponToUnlock)
    {
        weapons[weaponsUnlocked] = weaponToUnlock;
        weaponsUnlocked++;
    }
}
