using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponData weaponToUnlock;
    [Tooltip("If this is set, then instead of unlocking the WeaponToUnlock as a new item, this weapon gets replaced by WeaponToUnlock instead.")]
    [SerializeField] private WeaponData replaceWeapon;

    public void UnlockWeapon()
    {
        if(PlayerIdentifier.Instance.gameObject.TryGetComponent<WeaponSwappingHandler>(out WeaponSwappingHandler weaponSwapper))
        {
            weaponSwapper.UnlockWeapon(weaponToUnlock, replaceWeapon);
            Destroy(gameObject);
        }
    }
}
