using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerHealth : Health
{
    [Tooltip("If you want to change use ChangeRespawnPoint()")]
    [SerializeField]private Transform respawnPoint;
    [Tooltip("sphere that disables enemy aggro, in meters")]
    [SerializeField] private float disableRadius;
    [SerializeField] private LayerMask enemyLayer;
    protected override void OnDeath()
    {
        CharacterController controller = GetComponent<CharacterController>();

        currentHealth = maxHealth;
        UpdateHealthSlider();

        DisableTrigger();

        controller.enabled = false;
        transform.position = respawnPoint.position;
        controller.enabled = true;
        

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
                enemyMove.isFollowing = false;
            }
        }
    }

    public void ChangeRespawnPoint(Transform transform)
    {
        respawnPoint = transform;
    }
}
