using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioSource CombatAudioSource;
    [SerializeField] private AudioSource NeutralAudioSource;
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

    public void UpdateFollowing(int add)
    {
        enemiesFollowing += add;
        enemiesFollowing = Mathf.Clamp(enemiesFollowing, 0, 7);
        if (enemiesFollowing >= 1)
        {
            if (NeutralAudioSource.isPlaying == true)
            {
                NeutralAudioSource.Stop();
                Debug.Log("PlayingAudio " + enemiesFollowing);
                CombatAudioSource.Play();
            }
        }
        else if (enemiesFollowing == 0)
        {
            if (CombatAudioSource.isPlaying == true)
            {
                CombatAudioSource.Stop();
                Debug.Log("disabling audio");
                NeutralAudioSource.Play();
            }
        }
    }
}
