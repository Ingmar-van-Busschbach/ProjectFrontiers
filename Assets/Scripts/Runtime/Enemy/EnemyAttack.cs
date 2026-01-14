using Unity.VisualScripting;
using UnityEngine;


[RequireComponent (typeof(EnemyMove))]

public class EnemyAttack: MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] private EnumLibrary.EAttackType attackType;
    [SerializeField] private AreaAttack AreaAttackPrefab;
    [Header("Only for Area attack")]
    [SerializeField] private float attackCooldown = 10;

    private float timeOfNextAttack;
    private EnemyMove enemyMove;

    private void Awake()
    {
        enemyMove = GetComponent<EnemyMove>();
    }
    private void Update()
    {
        if(enemyMove.isFollowing == true)
        {
            HandleAttackType();
        }
    }
    void HandleAttackType()
    {

        switch(attackType)
        {
            case EnumLibrary.EAttackType.Area:
                HandleAreaAttack();
                break;

            case EnumLibrary.EAttackType.Homing:
                HandleHomingAttack();
                break;
        }

    }

    void HandleAreaAttack()
    {
        if (Time.time < timeOfNextAttack)
        {
            return;
        }
        timeOfNextAttack = Time.time + attackCooldown;
        Instantiate(AreaAttackPrefab, enemyMove.target.position, Quaternion.identity);
    }

    void HandleHomingAttack()
    {
        weapon.Shoot();
    }
}


