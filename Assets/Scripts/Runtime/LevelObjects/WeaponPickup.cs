using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] EnumLibrary.EWeaponPickup weaponPickup;

    public void UnlockWeapon()
    {
        if(PlayerIdentifier.Instance.gameObject.TryGetComponent<WeaponSwappingHandler>(out WeaponSwappingHandler weaponSwapper))
        {
            weaponSwapper.UnlockWeapon(weaponPickup);
            Destroy(gameObject);
        }
    }
}
