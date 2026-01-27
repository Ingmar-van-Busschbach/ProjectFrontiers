using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AttackAudioManager : MonoBehaviour
{
    public static AttackAudioManager instance;
    private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void PlayAudio(AudioClip clip, float volume, Vector3 position)
    {
        if (audioSource.isPlaying)
        {
            return;
        }
        transform.position = position;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }
}
