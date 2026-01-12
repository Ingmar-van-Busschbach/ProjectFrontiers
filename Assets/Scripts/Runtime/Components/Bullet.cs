using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour
{
    private WeaponData weaponData;
    private GameObject[] objectsToIgnore;
    private Rigidbody rigidBody;
    private Vector3 startLocation;

    public void Constructor(WeaponData weaponData, GameObject[] objectsToIgnore)
    {
        this.weaponData = weaponData;
        List<GameObject> list = objectsToIgnore.OfType<GameObject>().ToList();
        list.Add(gameObject);
        this.objectsToIgnore = list.ToArray();
        startLocation = transform.position;
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.linearVelocity = transform.forward * weaponData.velocity;
        rigidBody.useGravity = weaponData.bulletDrop;
    }

    private void Update()
    {
        if((transform.position - startLocation).magnitude > weaponData.maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Ignore the hit if it's in the objects to ignore list
        foreach (GameObject obj in objectsToIgnore)
        { 
            if(other.gameObject == obj)
            {
                return;
            }
        }


        if (weaponData.explosionMaxRadius > 0)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, weaponData.explosionMaxRadius);
            foreach (Collider hit in hitColliders)
            {
                bool shouldIgnore = false;
                foreach (GameObject obj in objectsToIgnore)
                {
                    if (hit.gameObject == obj)
                    {
                        shouldIgnore = true;
                    }
                }
                if (!shouldIgnore)
                {
                    ApplyHit(hit, transform.position, weaponData.explosionMaxRadius, weaponData.explosionOptimalRadius);
                }
            }
        }
        else
        {
            ApplyHit(other, startLocation, weaponData.maxRange, weaponData.optimalRange);
        }
        Destroy(gameObject);
    }

    private void ApplyHit(Collider other, Vector3 origin, float maxRange, float optimalRange)
    {   
        //We check if the target we hit can be damaged.
        if (other.gameObject.TryGetComponent<IDamageAble>(out IDamageAble target))
        {
            //Loop through all the different DamageData entries on the weapon.
            foreach (StructLibrary.Struct_DamageEntry damageData in weaponData.damageData)
            {
                //Standard linear damage falloff formula.
                float normalizedRange = ((other.ClosestPoint(transform.position) - origin).magnitude - optimalRange) / (maxRange - optimalRange);
                normalizedRange = Mathf.Clamp01(normalizedRange);
                float damage = damageData.maxDamage - (normalizedRange * (damageData.maxDamage - damageData.minDamage));

                //Round damage to whole number if rounding is enabled.
                if (weaponData.roundDamage)
                {
                    damage = Mathf.Round(damage);
                }

                //Apply to interface IDamageAble.
                target.ApplyDamage(damage, damageData.damageType, new RaycastHit());
            }
        }
    }
}
