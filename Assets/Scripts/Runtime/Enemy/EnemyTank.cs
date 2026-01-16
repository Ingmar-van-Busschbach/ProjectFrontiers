using UnityEngine;

public class EnemyTank : MonoBehaviour
{
    [SerializeField] private Transform visor;
    [SerializeField] private float fieldOfView;
    [SerializeField] private float viewDistance;
    [SerializeField] private LayerMask PlayerLayer;
    [SerializeField] private bool drawDebug;
         
    private Weapon weapon;

    private void Awake()
    {
        weapon = GetComponent<Weapon>();
    }
    private void FixedUpdate()
    {
        CheckLineOfSight();
    }
        private void CheckLineOfSight()
    {
        if (ConePhysics.ConeCast(out RaycastHit hit, visor.position, visor.forward, fieldOfView, 3, 0.5f, viewDistance, PlayerLayer, false, 0.1f, QueryTriggerInteraction.Ignore, drawDebug, Color.cyan))
        {
            weapon.Shoot();
        }

        else
        {
           return;
        }
    }
}

