using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Slider healSlider;
    [SerializeField] private AudioSource healAudioSource;
    private float timeOfNextHeal;
    private float timeOfLastHeal;
    private PlayerHealth playerHealth;
    

    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        healSlider.value = Mathf.Clamp01((Time.time - timeOfLastHeal) / healCooldown);
    }
    public void AttemptHeal()
    {
        if (Time.time < timeOfNextHeal)
        {
            return;
        }
        if (ManaManager.Instance.TryUseMana(manaUsage))
        {
            HandleCooldown();
            HandleHeal();
        }
    }

    private void HandleHeal()
    {
        playerHealth.ApplyDamage(-amountHealed, EnumLibrary.EDamageType.healing, new RaycastHit());
        healAudioSource.PlayOneShot(healAudio, audioVolume);
    }

    private void HandleCooldown()
    {
        timeOfNextHeal = Time.time + healCooldown;
        timeOfLastHeal = Time.time;
    }
}
