using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource CombatAudioSource;
    [SerializeField] private AudioSource NeutralAudioSource;
    public static AudioManager instance;
    private void Awake()
    {
        if (instance != null)
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
    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.enemiesFollowing >= 1)
        {
            if (NeutralAudioSource.isPlaying == true)
            {
                NeutralAudioSource.Stop();
                Debug.Log("PlayingAudio");
                CombatAudioSource.Play();
            }
        }
        else if (GameManager.instance != null && GameManager.instance.enemiesFollowing == 0)
        {
            if (CombatAudioSource.isPlaying == true)
            {
                CombatAudioSource.Stop();
                NeutralAudioSource.Play();
            }
        }
    }
}
