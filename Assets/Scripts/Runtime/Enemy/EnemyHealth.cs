using UnityEngine;

public class EnemyHealth : Health
{
    [SerializeField] private DamageNumber damageNumber;
    protected override void OnDeath()
    {
        Destroy(gameObject);
        if(GameManager.instance != null)
        {
            GameManager.instance.UpdateFollowing(-1);
        }
    }

    protected override void OnDamaged(float damage, RaycastHit hitData)
    {
        DamageNumber dmgNumber = Instantiate(damageNumber, hitData.point + hitData.normal, Quaternion.identity);
        dmgNumber.Constructor(damage);
        if(gameObject.TryGetComponent<EnemyMove>(out EnemyMove enemyMove))
        {
            if (enemyMove.isFollowing == false)
            {
                enemyMove.isFollowing = true;
                GameManager.instance.UpdateFollowing(1);
                enemyMove.target = PlayerIdentifier.Instance.gameObject.transform;
                enemyMove.currentTimeBeforeReturnPatrol = enemyMove.timeBeforeReturnPatrol;
            }
        }
    }

    protected override void BeginPlay()
    {
    }
}
