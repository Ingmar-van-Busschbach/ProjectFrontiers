using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource CombatAudioSource;
    [SerializeField] private AudioSource NeutralAudioSource;
    [SerializeField] private float blendDuration = 2;
    public static GameManager instance;
    public int enemiesFollowing;
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
    }

    private void OnDestroy()
    {
        Destroy(this);
    }

    private void Update()
    {
        if(enemiesFollowing >= 1)
        {
            if (!CombatAudioSource.isPlaying)
            {
                CombatAudioSource.Play();
            }
            CombatAudioSource.volume = Mathf.Clamp01(CombatAudioSource.volume + Time.deltaTime / blendDuration);
            NeutralAudioSource.volume = Mathf.Clamp01(NeutralAudioSource.volume - Time.deltaTime / blendDuration);
            if(NeutralAudioSource.volume <= 0)
            {
                NeutralAudioSource.Stop();
            }
        }
        else
        {
            if (!NeutralAudioSource.isPlaying)
            {
                NeutralAudioSource.Play();
            }
            NeutralAudioSource.volume = Mathf.Clamp01(NeutralAudioSource.volume + Time.deltaTime / blendDuration);
            CombatAudioSource.volume = Mathf.Clamp01(CombatAudioSource.volume - Time.deltaTime / blendDuration);
            if (NeutralAudioSource.volume <= 0)
            {
                CombatAudioSource.Stop();
            }
        }
    }

    public void UpdateFollowing(int add)
    {
        enemiesFollowing += add;
        enemiesFollowing = Mathf.Clamp(enemiesFollowing, 0, 5);
    }
}
