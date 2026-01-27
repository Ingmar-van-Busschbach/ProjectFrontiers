using UnityEngine;
using System.Collections;


[RequireComponent (typeof(EnemyMove))]

public class EnemyAttack: MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] private EnumLibrary.EAttackType attackType;
    [SerializeField] private AreaAttack AreaAttackPrefab;
    [SerializeField] private Animator animator;
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
        animator.SetTrigger("Attack02");
    }

    void HandleHomingAttack()
    {
        if (Time.time < timeOfNextAttack)
        {
            return;
        }
        timeOfNextAttack = Time.time + attackCooldown;
        StartCoroutine(DelayShot());
        animator.SetTrigger("Attack01");
        
    }

    private IEnumerator DelayShot()
    {
        yield return new WaitForSeconds(0.3f);
        weapon.Shoot();
    }
}


