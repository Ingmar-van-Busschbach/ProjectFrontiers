using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(AudioSource))]
public class HealHandler : MonoBehaviour
{
    [Tooltip("Uses negative damage to heal. The value should be positive as it gets inverted.")]
    [SerializeField] private float amountHealed;
    [Tooltip("Cooldown between heals in seconds.")]
    [SerializeField] private float healCooldown;
    [Tooltip("The amount of mana the heal uses")]
    [SerializeField] private float manaUsage;
    [SerializeField] private AudioClip healAudio;
    [SerializeField] private float audioVolume;
    private float timeOfNextHeal;
    private PlayerHealth playerHealth;
    private AudioSource healAudioSource;

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        healAudioSource = GetComponent<AudioSource>();
    }
    public void AttemptHeal()
    {
        if (Time.time < timeOfNextHeal)
        {
            return;
        }
        timeOfNextHeal = Time.time + healCooldown;
        if (ManaManager.Instance.TryUseMana(manaUsage))
        {
            HandleHeal();
        }
    }

    private void HandleHeal()
    {
        playerHealth.ApplyDamage(-amountHealed, EnumLibrary.EDamageType.healing, new RaycastHit());
        healAudioSource.PlayOneShot(healAudio, audioVolume);
    }
}
