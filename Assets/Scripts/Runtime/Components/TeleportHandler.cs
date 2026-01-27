using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class TeleportHandler : MonoBehaviour
{
    [Tooltip("Should be the player camera.")]
    [SerializeField] private Camera targetingOrigin;
    [Tooltip("Range of the raycast that the telporter uses to determine where to teleport to.")]
    [SerializeField] private float teleportRange;
    [Tooltip("Layers to test for when making the raycast.")]
    [SerializeField] private LayerMask teleportLayerMask;
    [Tooltip("The distance in meters that the teleporting character should be moved away from the surface it is attempting to teleport to. This is to prevent the teleporting character from clipping into objects.")]
    [SerializeField] private float teleportNormalOffset;
    [Tooltip("Cooldown between teleports in seconds.")]
    [SerializeField] private float teleportCooldown;
    [Tooltip("The amount of mana the teleport uses")]
    [SerializeField] private float manaUsage;
    [SerializeField] private AudioClip teleportAudio;
    [SerializeField] private float audioVolume;
    private float timeOfNextTeleport;
    private AudioSource teleportAudioSource;

    private void Start()
    {
        teleportAudioSource = GetComponent<AudioSource>();
    }
    public void AttemptTeleport()
    {
        if (Time.time < timeOfNextTeleport)
        {
            return;
        }
        timeOfNextTeleport = Time.time + teleportCooldown;

        //Teleport to the point the mouse aims at
        if (Physics.Raycast(targetingOrigin.transform.position, targetingOrigin.transform.forward, out RaycastHit hit, teleportRange, teleportLayerMask, QueryTriggerInteraction.Ignore))
        {
            if (ManaManager.Instance.TryUseMana(manaUsage))
            {
                HandleTeleport(hit);
            }
        }
        else
        {
            //Else teleport to the floor underneath where the mouse aims at
            if (Physics.Raycast(targetingOrigin.transform.position + targetingOrigin.transform.forward * teleportRange, Vector3.down, out RaycastHit groundHit, Mathf.Infinity, teleportLayerMask, QueryTriggerInteraction.Ignore))
            {
                if (ManaManager.Instance.TryUseMana(manaUsage))
                {
                    HandleTeleport(groundHit);
                }
            }
        }
    }

    private void HandleTeleport(RaycastHit hit)
    {
        teleportAudioSource.PlayOneShot(teleportAudio, audioVolume);
        Vector3 teleportLocation = hit.point;
        teleportLocation += hit.normal * teleportNormalOffset;
        CharacterController controller = GetComponent<CharacterController>();
        controller.enabled = false;
        transform.position = teleportLocation;
        controller.enabled = true;
    }
}
