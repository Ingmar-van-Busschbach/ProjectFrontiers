using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyMove : MonoBehaviour, IAlert
{
    [SerializeField] private LayerMask PlayerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private Animator animator;
    public float timeBeforeReturnPatrol = 3;
    [Tooltip("m/s")]
    [SerializeField] private float speed = 2;
    [SerializeField] private bool drawDebug;


    [Header("Patrol")]
    [SerializeField] List<Transform> Locations;
    
    [Header("Line of sight")]
    [Tooltip("Angle in degrees")]
    [SerializeField] private float fieldOfView;
    [Tooltip("Length of field of view")]
    [SerializeField] private float viewDistance;
    [Tooltip("enemy doesn't go closer to player then this")]
    [SerializeField] private float minDistanceToPlayer;


    [Header("Alert")]
    [Tooltip("Radius of the EnemyAlarm")]
    [SerializeField] private float alarmRadius;

    [Header("Gravity")]
    [Tooltip("m/s^2")]
    [SerializeField] private float gravityStrength = 2f;
    [Tooltip("m/s")]
    [SerializeField] private float maxFallSpeed = 10f;

    private int currentLocation;
    private int nextLocation;
    private float acceptanceRadius = 0.7f;
    private float verticalVelocity;
    [HideInInspector] public float currentTimeBeforeReturnPatrol;
    [HideInInspector] public float currentTimeBeforeLOSCheck = 0;

    private CharacterController controller;

    [HideInInspector] public Transform target;
    [HideInInspector] public bool isFollowing = false;


    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }


    private void FixedUpdate()
    {
        currentTimeBeforeLOSCheck -= Time.fixedDeltaTime;
        if (currentTimeBeforeLOSCheck <= 0)
        {
            CheckLineOfSight();
        }
        else
        {
            FailedLineOfSight();

        }

        if (isFollowing)
        {
            EnemyAlarm();
            float distance = Vector3.Distance(transform.position, target.position);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            HandleGravity();
            controller.Move(Time.deltaTime * new Vector3(0, verticalVelocity, 0));
            if (distance > minDistanceToPlayer)
            {
                Vector3 direction = (target.position - transform.position);
                direction.y = 0;
                if(direction.magnitude > 1)
                {
                    direction.Normalize();
                }
                animator.SetFloat("Velocity", direction.magnitude);
                controller.Move(direction * Time.deltaTime * speed);
            }
            else
            {
                animator.SetFloat("Velocity", 0);
            }
        }
        else
        {
            if (Locations.Count == 0 || Locations == null)
            {
                return;
            }
            if (currentLocation < Locations.Count)
            {
                GoToLocation();
            }
            else
            {
                currentLocation = 0;
            }
        }
    }

    private void GoToLocation()
    {
        if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(Locations[nextLocation].position.x, 0, Locations[nextLocation].position.z)) < acceptanceRadius)
        {
            currentLocation = nextLocation;
            nextLocation = (currentLocation + 1) % Locations.Count;
        }
        Vector3 lookDirection = Locations[nextLocation].position - transform.position;
        lookDirection.y = 0; // never look down or up
        lookDirection.Normalize();
        Vector3 upDirection = Vector3.up;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookDirection, upDirection), 0.2f);
        Vector3 moveDirection = lookDirection * Time.deltaTime * speed;
        if ((moveDirection).magnitude > (Locations[nextLocation].position - transform.position).magnitude)
        {
            moveDirection = Locations[nextLocation].position - transform.position;
        }
        animator.SetFloat("Velocity", (lookDirection.magnitude));
        controller.Move(moveDirection);
    }
    private void CheckLineOfSight()
    {
        if (ConePhysics.ConeCast(out RaycastHit hit, eyeTransform.position, eyeTransform.forward, fieldOfView, 3, 0.5f, viewDistance, PlayerLayer, false, 0.1f, QueryTriggerInteraction.Ignore, drawDebug, Color.cyan))
        {
            if (Physics.Raycast(eyeTransform.position, hit.point - eyeTransform.position, out RaycastHit obstructionCheck))
            {
                if(obstructionCheck.collider.gameObject == PlayerIdentifier.Instance.gameObject)
                {
                    if (!isFollowing)
                    {
                        GameManager.instance.UpdateFollowing(1);
                        isFollowing = true;
                        target = hit.collider.transform;
                        currentTimeBeforeReturnPatrol = timeBeforeReturnPatrol;
                    }
                }
            }

        }

        else
        {
            FailedLineOfSight();
        }
    }
    private void HandleGravity()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -0.1f; // keep grounded
        }
        else
        {
            if (verticalVelocity < maxFallSpeed)
            {
                verticalVelocity -= gravityStrength * Time.fixedDeltaTime;
            }
        }
    }

    private void EnemyAlarm()
    {
        Collider[] alarmCollider = Physics.OverlapSphere(transform.position, alarmRadius, enemyLayer, QueryTriggerInteraction.Ignore);
        foreach (Collider col in alarmCollider)
        {
            if (col.gameObject != gameObject)
            {
                if (col.gameObject.TryGetComponent<EnemyMove>(out EnemyMove enemyMove))
                {
                    HandleAlert(target);
                }
            }
        }
    }

    public void HandleAlert(Transform target)
    {
        // handles target change
        
        if (!isFollowing)
        {
            this.target = target;
            GameManager.instance.UpdateFollowing(1);
            isFollowing = true;
            currentTimeBeforeReturnPatrol = timeBeforeReturnPatrol;
        }

    }

    private void FailedLineOfSight()
    {
        currentTimeBeforeReturnPatrol -= Time.fixedDeltaTime;
        if (currentTimeBeforeReturnPatrol < 0)
        {
            if (isFollowing)
            {
                GameManager.instance.UpdateFollowing(-1);
                isFollowing = false;
            }
        }
    }

}
