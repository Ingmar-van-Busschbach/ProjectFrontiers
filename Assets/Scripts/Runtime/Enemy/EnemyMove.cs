using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyMove : MonoBehaviour
{
    [SerializeField] private LayerMask PlayerLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform eyeTransform;
    [SerializeField] private float timeBeforeReturnPatrol = 3;
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
    private float currentTimeBeforeReturnPatrol;

    private CharacterController controller;

    [HideInInspector] public Transform target;
    [HideInInspector] public bool isFollowing = false;

    

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    private void FixedUpdate()
    {
        CheckLineOfSight();

        if (isFollowing) 
        {
            Debug.Log(gameObject.name);
            EnemyAlarm();
            float distance = Vector3.Distance(transform.position, target.position);
            transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
            HandleGravity();
            controller.Move(Time.deltaTime * new Vector3(0, verticalVelocity, 0));
            if (distance > minDistanceToPlayer)
            {
                Vector3 direction = (target.position - transform.position);
                direction.y = 0;
                controller.Move(direction.normalized * Time.deltaTime * speed);

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

        transform.rotation = Quaternion.LookRotation(lookDirection, upDirection);
        Vector3 moveDirection = lookDirection * Time.deltaTime * speed;
        if ((moveDirection).magnitude > (Locations[nextLocation].position - transform.position).magnitude)
        {
            moveDirection = Locations[nextLocation].position - transform.position;
        }

        controller.Move(moveDirection);
    }
    private void CheckLineOfSight()
    {
        if (ConePhysics.ConeCast(out RaycastHit hit, eyeTransform.position, eyeTransform.forward, fieldOfView, 3, 0.5f, viewDistance, PlayerLayer, false, 0.1f, QueryTriggerInteraction.Ignore, drawDebug, Color.cyan))
        {
            isFollowing = true;
            target = hit.collider.transform;
            currentTimeBeforeReturnPatrol = timeBeforeReturnPatrol;
        }

        else 
        {
            currentTimeBeforeReturnPatrol -= Time.fixedDeltaTime;
            if (currentTimeBeforeReturnPatrol < 0)
            {
                isFollowing = false;
            }
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
                    Debug.Log(gameObject.name + " is triggering alarm of " + col.gameObject.name);
                    enemyMove.HandleAlert(target);
                }
            }
        }
    }

    public void HandleAlert(Transform newTarget)
    {
        target = newTarget;
        Debug.Log(target);
        isFollowing = true;
        currentTimeBeforeReturnPatrol = timeBeforeReturnPatrol;
    }
}
