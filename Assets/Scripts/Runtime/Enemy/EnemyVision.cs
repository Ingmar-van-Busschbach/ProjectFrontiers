using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyVision : MonoBehaviour
{
    public Transform player;
    public float visionRange = 15f;
    public float detectionSpeed = 1f;      // how fast the bar fills
    public float decaySpeed = 0.5f;        // how fast the bar decreases
    public float maxDetection = 5f;        // max counter

    private bool playerInCollider = false;
    private bool dialogueTriggered = false;
    public float detectionCounter = 0f;



    void Update()
    {
        if (CanSeePlayer())
        {
            detectionCounter += detectionSpeed * Time.deltaTime; //detection goes up over time when the player is in sight
        }
        else
        {
            detectionCounter -= decaySpeed * Time.deltaTime;
        }

        detectionCounter = Mathf.Clamp(detectionCounter, 0, maxDetection); //detectionCounter should be between 0 and maxDetection

        if (detectionCounter >= maxDetection && dialogueTriggered==false)
        {
            dialogueTriggered = true;  
            detectionCounter = 0f;    // reset
        }
        else
        {
            dialogueTriggered = false; //reset
        }

    }

    bool CanSeePlayer()
    {
        if(playerInCollider == true)
        {
            Vector3 lookDirection = player.position - transform.position; // difference vector (see paint)
            lookDirection.y = 0; // never look down or up (locking rotation)
            Vector3 upDirection = Vector3.up;

            transform.rotation =
                Quaternion.LookRotation(lookDirection, upDirection); 


            // Raycast
            if (Physics.Raycast(transform.position,
                                transform.forward,
                                out RaycastHit hit,
                                visionRange))

            {
                if (hit.collider.CompareTag("PlayerRayCollider")) 
                {
                    Debug.DrawLine(transform.position, hit.point, Color.red);

                    //Debug.Log("PlayerHit");
                    return true;
                }
                else
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.forward * visionRange, Color.blue);

            }
        }
        return false;
    }

    void OnTriggerEnter(Collider other)  //make sure enemy only looks when player is in a certain range
    {
        
        if (other.CompareTag("PlayerRayCollider"))
        {
            playerInCollider = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("PlayerRayCollider") && detectionCounter < 0)
        {
            playerInCollider = false;
            transform.rotation = Quaternion.Euler(0, 0, 0); //set rotation back to origin
        }
        else if (other.CompareTag("PlayerRayCollider") && detectionCounter > 0)
        {
            playerInCollider = true;
        }
    }

    private void OnTriggerExit(Collider other) //stop looking when player out of range
    {
        if (other.CompareTag("PlayerRayCollider"))
        {
            playerInCollider = false;
        }
    }
}

