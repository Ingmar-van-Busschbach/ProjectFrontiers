using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponData weaponToUnlock;

    public void UnlockWeapon()
    {
        if(PlayerIdentifier.Instance.gameObject.TryGetComponent<WeaponSwappingHandler>(out WeaponSwappingHandler weaponSwapper))
        {
            weaponSwapper.UnlockWeapon(weaponToUnlock);
            Destroy(gameObject);
        }
    }
}
