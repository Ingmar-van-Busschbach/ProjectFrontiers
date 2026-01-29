using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AttackAudioManager : MonoBehaviour
{
    public static AttackAudioManager instance;
    private AudioSource audioSource;
    private Transform targetTransform;

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

    private void Update()
    {
        if(targetTransform != null)
        {
            transform.position = targetTransform.position;
        }
    }

    public void PlayAudio(AudioClip clip, float volume, Transform targetTransform)
    {
        if (audioSource.isPlaying)
        {
            return;
        }
        this.targetTransform = targetTransform;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }
}
