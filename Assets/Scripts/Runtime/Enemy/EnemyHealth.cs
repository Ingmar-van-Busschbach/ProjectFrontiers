using UnityEngine;

public class EnemyHealth : Health
{
    protected override void OnDeath()
    {
        Destroy(gameObject);
        GameManager.instance.UpdateFollowing(-1);
    }

    protected override void OnDamaged(float damage)
    {
        if (enemyMove.isFollowing == false)
        {
            enemyMove.isFollowing = true;
            Debug.Log("ondamaged");
            GameManager.instance.UpdateFollowing(1);
            enemyMove.target = PlayerIdentifier.Instance.gameObject.transform;
            enemyMove.currentTimeBeforeReturnPatrol = enemyMove.timeBeforeReturnPatrol;
        }

    }
}
