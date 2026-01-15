using UnityEngine;

public class EnemyHealth : Health
{
    private EnemyMove enemyMove;

    private void Awake()
    {
        enemyMove = GetComponent<EnemyMove>();
    }
    protected override void OnDeath()
    {
        Destroy(gameObject);
    }

    protected override void OnDamaged(float damage)
    {
        enemyMove.isFollowing = true;
        enemyMove.target = PlayerIdentifier.Instance.gameObject.transform;
        enemyMove.currentTimeBeforeReturnPatrol = enemyMove.timeBeforeReturnPatrol;

    }
}
