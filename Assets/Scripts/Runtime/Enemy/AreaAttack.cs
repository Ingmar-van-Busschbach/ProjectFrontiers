using System;
using Unity.Collections;
using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class AreaAttack : MonoBehaviour
{
    [Tooltip("Time between casting and damage")]
    [SerializeField] private float timeToDamage;
    [SerializeField] private float damageRadius;
    [SerializeField] private float damagePerSecond;
    [SerializeField] private float damageDuration;
    
    [SerializeField] LayerMask layerToHit;
    [SerializeField] EnumLibrary.EDamageType damageType;

    private float timeElapsed;

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed < timeToDamage)
        {
            return;
        }
            
        Collider[] damageCollider = Physics.OverlapSphere(transform.position, damageRadius, layerToHit, QueryTriggerInteraction.Ignore);
        foreach (Collider col in damageCollider)
        {
            if (col.gameObject.TryGetComponent<IDamageAble>(out IDamageAble target))
            {
                target.ApplyDamage(damagePerSecond, damageType, new RaycastHit());
            }
        }

        Destroy(gameObject);
    }
}

