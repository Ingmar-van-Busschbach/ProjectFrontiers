using UnityEngine;

public class ProjectileWeapon : Weapon
{
    protected override void HandleShot()
    {
        Bullet bullet = Instantiate(weaponData.bullet, barrelPoint.position, barrelPoint.rotation * HandleDispersion());
        bullet.Constructor(weaponData, objectsToIgnore);
    }
}
