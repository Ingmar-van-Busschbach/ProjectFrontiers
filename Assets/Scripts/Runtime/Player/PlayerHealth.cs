using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerHealth : Health
{
    [Tooltip("If you want to change use ChangeRespawnPoint()")]
    [SerializeField]private Transform respawnPoint;
    [Tooltip("sphere that disables enemy aggro, in meters")]
    [SerializeField] private float disableRadius;
    [SerializeField] private LayerMask enemyLayer;

    private void Start()
    {
        base.Start();
        if (respawnPoint == null)
        {
            respawnPoint = Instantiate(new GameObject(), gameObject.transform).transform;
        }
        //StartCoroutine(die());
    }
    private IEnumerator die()
    {
        yield return new WaitForSeconds(5);
        this.ApplyDamage(100, EnumLibrary.EDamageType.impact, new RaycastHit());
    }
    protected override void OnDeath()
    {
        CharacterController controller = GetComponent<CharacterController>();
        DisableTrigger();

        controller.enabled = false;
        transform.position = respawnPoint.position;
        controller.enabled = true;

        currentHealth = maxHealth;
        UpdateHealthSlider();

        

    }
    protected override void OnDamaged(float damage)
    {
        
    }

    private void DisableTrigger()
    {
        Collider[] triggerCollider = Physics.OverlapSphere(transform.position, disableRadius, enemyLayer, QueryTriggerInteraction.Ignore);
        foreach (Collider col in triggerCollider)
        {
            if (col.gameObject.TryGetComponent<EnemyMove>(out EnemyMove enemyMove))
            {
                if (enemyMove.isFollowing == true)
                {
                    enemyMove.isFollowing = false;
                    enemyMove.currentTimeBeforeReturnPatrol = 0;
                    GameManager.instance.UpdateFollowing(-1);
                }
                
            }
        }
    }

    public void ChangeRespawnPoint(Transform transform)
    {
        respawnPoint = transform;
    }
}
