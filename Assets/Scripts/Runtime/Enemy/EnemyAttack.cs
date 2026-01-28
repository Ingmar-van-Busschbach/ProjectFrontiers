using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;


[RequireComponent(typeof(EnemyMove))]

public class EnemyAttack: MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] private EnumLibrary.EAttackType attackType;
    [SerializeField] private AreaAttack AreaAttackPrefab;
    [SerializeField] private Animator animator;
    [SerializeField] private float attackCooldown = 10;
    [SerializeField] private RandomAudioContainer randomAttackAudio;

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
        AttackAudio();
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
        AttackAudio();
        weapon.Shoot();
    }

    private void AttackAudio()
    {
        float totalWeight = 0;
        for(int i = 0; i < randomAttackAudio.randomAudio.Length; i++)
        {
            totalWeight += randomAttackAudio.randomAudio[i].weight;
        }
        float randomWeight = Random.Range(0, totalWeight);
        totalWeight = 0;
        for (int i = 0; i < randomAttackAudio.randomAudio.Length; i++)
        {
            totalWeight += randomAttackAudio.randomAudio[i].weight;
            if(randomWeight < totalWeight)
            {
                AttackAudioManager.instance.PlayAudio(randomAttackAudio.randomAudio[i].audioClip, randomAttackAudio.randomAudio[i].volume, transform);
                return;
            }
        }
    }
}


