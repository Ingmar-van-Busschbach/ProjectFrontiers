using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    public Transform sightTransform;
    public Animator animator;
    public void PlayFiringAnimation()
    {
        animator.SetTrigger("Shoot");
    }
}
