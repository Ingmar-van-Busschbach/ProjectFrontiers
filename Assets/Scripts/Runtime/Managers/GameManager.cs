using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource combatAudioSource;
    [SerializeField] private AudioSource neutralAudioSource;
    [SerializeField] private float blendDuration = 2;
    public static GameManager instance;
    public int enemiesFollowing;
    private float combatAudioVolume;
    private float neutralAudioVolume;
    private void Awake()
    {
        if (instance  != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        combatAudioVolume = combatAudioSource.volume;
        neutralAudioVolume = neutralAudioSource.volume;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    private void Update()
    {
        if(enemiesFollowing >= 1)
        {
            if (!combatAudioSource.isPlaying)
            {
                combatAudioSource.Play();
            }
            combatAudioSource.volume = Mathf.Clamp(combatAudioSource.volume + Time.deltaTime * combatAudioVolume / blendDuration, 0, combatAudioVolume);
            neutralAudioSource.volume = Mathf.Clamp(neutralAudioSource.volume - Time.deltaTime * neutralAudioVolume / blendDuration, 0, neutralAudioVolume);
            if(neutralAudioSource.volume <= 0)
            {
                neutralAudioSource.Stop();
            }
        }
        else
        {
            if (!neutralAudioSource.isPlaying)
            {
                neutralAudioSource.Play();
            }
            neutralAudioSource.volume = Mathf.Clamp(neutralAudioSource.volume + Time.deltaTime * neutralAudioVolume / blendDuration, 0, neutralAudioVolume);
            combatAudioSource.volume = Mathf.Clamp(combatAudioSource.volume - Time.deltaTime * combatAudioVolume / blendDuration, 0, combatAudioVolume);
            if (neutralAudioSource.volume <= 0)
            {
                combatAudioSource.Stop();
            }
        }
    }

    public void UpdateFollowing(int add)
    {
        enemiesFollowing += add;
        enemiesFollowing = Mathf.Clamp(enemiesFollowing, 0, 5);
    }
}
