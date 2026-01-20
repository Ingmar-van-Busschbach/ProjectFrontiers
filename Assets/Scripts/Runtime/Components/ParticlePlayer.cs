using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticlePlayer : MonoBehaviour
{
    private ParticleSystem particleSys;
    void Start()
    {
        particleSys = GetComponent<ParticleSystem>();
        particleSys.Play();
        Destroy(gameObject, particleSys.main.duration);
    }
}
