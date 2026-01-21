using System.Collections;
using UnityEngine;


public class AreaAttack : MonoBehaviour
{
    [Tooltip("Time between casting and damage")]
    [SerializeField] private float timeToDamage;
    [SerializeField] private float damageRadius;
    [SerializeField] private float damagePerSecond;
    [SerializeField] private float damageDuration = 3;
    [Tooltip("The amount of times damage/second is divided (the higher the smoother)")]
    [SerializeField] private float ticksPerSecond = 10;
    
    [SerializeField] LayerMask layerToHit;
    [SerializeField] EnumLibrary.EDamageType damageType;
    private bool startedDamage;

    private float timeElapsed;

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed < timeToDamage)
        {
            return;
        }
        if (!startedDamage)
        {
            startedDamage = true;
            StartCoroutine(DealDamage());
        }

    }

    private IEnumerator DealDamage()
    {
        
        float currentDuration = 0;
        while (currentDuration <= damageDuration)
        {
            currentDuration += 1 / ticksPerSecond;
            Collider[] damageCollider = Physics.OverlapSphere(transform.position, damageRadius, layerToHit, QueryTriggerInteraction.Ignore);
            foreach (Collider col in damageCollider)
            {
                if (col.gameObject.TryGetComponent<IDamageAble>(out IDamageAble target))
                {
                    target.ApplyDamage(damagePerSecond / ticksPerSecond, damageType, new RaycastHit());

                }
            }
            yield return new WaitForSeconds(1 / ticksPerSecond);
        }

        Destroy(gameObject);
    }
}

