using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/RandomAudioContainer", order = 1)]
public class RandomAudioContainer : ScriptableObject
{
    public StructLibrary.Struct_RandomAudioEntry[] randomAudio;
}
