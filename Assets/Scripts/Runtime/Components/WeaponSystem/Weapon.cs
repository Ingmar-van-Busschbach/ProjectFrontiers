using UnityEngine;

/// <summary>
/// Player weapon script. Implements weapon accuracy, a cone cast aim assist, linear damage falloff, damage types and hit data.
/// </summary>

public class Weapon : ShotHandler
{
    protected override void HandleShot()
    {
        Quaternion dispersion = HandleDispersion();
        Quaternion aim = dispersion;

        //Handle weapon firing particles
        if (weaponData.muzzleFlash != null)
        {
            GameObject muzzleFlash = Instantiate(weaponData.muzzleFlash, barrelPoint.position, Quaternion.LookRotation(dispersion * barrelPoint.forward));
            muzzleFlash.transform.parent = barrelPoint;
        }
        if (weaponData.tracer != null)
        {
            Instantiate(weaponData.tracer, barrelPoint.position, Quaternion.LookRotation(dispersion * barrelPoint.forward));
        }
        if(weaponObject != null)
        {
            weaponObject.PlayFiringAnimation();
        }

        //Alert nearby enemies when shooting
        if (weaponData.enemyAlertMask != 0)
        {
            Collider[] colliders = Physics.OverlapSphere(barrelPoint.position, weaponData.alertRadius, weaponData.enemyAlertMask, QueryTriggerInteraction.Ignore);
            foreach (Collider collider in colliders)
            {
                if(collider.TryGetComponent<IAlert>(out IAlert alertAble))
                {
                    alertAble.HandleAlert(gameObject.transform);
                }
            }
        }
        switch (weaponData.weaponType)
        {
            case EnumLibrary.EWeaponType.hitscan:
                HandleHitscanShot(aim);
                break;
            case EnumLibrary.EWeaponType.projectile:
                HandleProjectileShot(aim);
                break;
        }
    }

    private void HandleHitscanShot(Quaternion dispersion)
    {
        // We use a cone cast if we are using aim assist. Otherwise we use a simple raycast.
        RaycastHit[] hitResults = null;
        if (weaponData.aimAssist > 0)
        {
            if (weaponData.penetratesTargets)
            {
                hitResults = ConePhysics.ConeCastAll(barrelPoint.position, dispersion * barrelPoint.forward, weaponData.aimAssist, weaponData.aimAssistFidelity, weaponData.minRange, weaponData.maxRange, weaponData.layerMask, true, 0.1f, QueryTriggerInteraction.Ignore, debugEnabled, Color.white);
            }
            else
            {
                if(ConePhysics.ConeCast(out RaycastHit hit, barrelPoint.position, dispersion * barrelPoint.forward, weaponData.aimAssist, weaponData.aimAssistFidelity, weaponData.minRange, weaponData.maxRange, weaponData.layerMask, true, 0.1f, QueryTriggerInteraction.Ignore, debugEnabled, Color.white))
                {
                    hitResults = new RaycastHit[1];
                    hitResults[0] = hit;
                }
            }
        }
        else
        {
            if (weaponData.penetratesTargets)
            {
                hitResults = Physics.RaycastAll(barrelPoint.position + dispersion * barrelPoint.forward * weaponData.minRange, dispersion * barrelPoint.forward, weaponData.maxRange, weaponData.layerMask, QueryTriggerInteraction.Ignore);
            }
            else
            {
                if (Physics.Raycast(barrelPoint.position + dispersion * barrelPoint.forward * weaponData.minRange, dispersion * barrelPoint.forward, out RaycastHit hit, weaponData.maxRange, weaponData.layerMask, QueryTriggerInteraction.Ignore))
                {
                    hitResults = new RaycastHit[1];
                    hitResults[0] = hit;
                }
            }
            if (debugEnabled)
            {
                Debug.DrawLine(barrelPoint.position + dispersion * barrelPoint.forward * weaponData.minRange, barrelPoint.position + dispersion * barrelPoint.forward * weaponData.maxRange, Color.white, 0.1f);
            }
        }

        

        //Abort if there are no hits.
        if (hitResults.Length <= 0)
        {
            return;
        }

        //If we penetrate targets, we apply a hit to every target in the raycast.
        if (weaponData.penetratesTargets)
        {
            foreach (RaycastHit hit in hitResults)
            {
                OnHit(hit);
            }
        }
        //Otherwise, we filter the hit results for the target most close to the weapon's origin that can also be damaged, then apply a hit to that.
        else
        {
            float currentHitDistance = Mathf.Infinity;
            RaycastHit currentHit = new RaycastHit();
            foreach (RaycastHit hit in hitResults)
            { //Check if the target is both closer than any previous target, and is DamageAble.
                if (hit.distance < currentHitDistance && hit.collider.gameObject.TryGetComponent<IDamageAble>(out IDamageAble target))
                {
                    currentHitDistance = hit.distance;
                    currentHit = hit;
                }
            }
            if (currentHitDistance < Mathf.Infinity)
            {
                OnHit(currentHit);
            }
        }
    }

    private void HandleProjectileShot(Quaternion dispersion)
    {
        Bullet bullet = Instantiate(weaponData.bullet, barrelPoint.position, barrelPoint.rotation * dispersion);
        bullet.Constructor(weaponData, objectsToIgnore);
    }

    private void OnHit(RaycastHit hitData)
    {
        //We check if the target we hit can be damaged.
        if (hitData.collider.gameObject.TryGetComponent<IDamageAble>(out IDamageAble target))
        {
            //Loop through all the different DamageData entries on the weapon.
            foreach (StructLibrary.Struct_DamageEntry damageData in weaponData.damageData)
            {
                //Standard linear damage falloff formula.
                float normalizedRange = (hitData.distance - weaponData.optimalRange) / (weaponData.maxRange - weaponData.optimalRange);
                normalizedRange = Mathf.Clamp01(normalizedRange);
                float damage = damageData.maxDamage - (normalizedRange * (damageData.maxDamage - damageData.minDamage));

                //Round damage to whole number if rounding is enabled.
                if (weaponData.roundDamage)
                {
                    damage = Mathf.Round(damage);
                }

                //Apply to interface IDamageAble.
                target.ApplyDamage(damage, damageData.damageType, hitData);
            }
        }
    }
}