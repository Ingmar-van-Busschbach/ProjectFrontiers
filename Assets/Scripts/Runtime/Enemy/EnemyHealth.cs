using UnityEngine;

public class EnemyHealth : Health
{
    protected override void OnDeath()
    {
        Destroy(gameObject);
    }

    protected override void OnDamaged(float damage)
    {
        if(gameObject.TryGetComponent<IAlert>(out IAlert iAlert))
        {
            iAlert.HandleAlert(PlayerIdentifier.Instance.gameObject.transform);
        }
    }
}
