using UnityEngine;

public class EnemyHealth : Health
{
    protected override void OnDeath()
    {
        Destroy(gameObject);
    }

    protected override void OnDamaged(float damage)
    {
         
    }
}
