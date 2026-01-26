using UnityEngine;

public class StructLibrary : MonoBehaviour
{
    [System.Serializable]
    public struct Struct_DamageEntry
    {
        [Tooltip("Damage at no falloff")]
        public float maxDamage;
        [Tooltip("Damage at max falloff")]
        public float minDamage;
        public EnumLibrary.EDamageType damageType;
    }

    [System.Serializable]

    public struct Struct_ResistanceEntry
    {
        [Tooltip("Damage multiplier >0 = more damage <0 = less damage")]
        public float damageMultiplier;
        public EnumLibrary.EDamageType resistanceType;
    }

    [System.Serializable]
    public struct Struct_DialogueEntry
    {
        [Tooltip("Who is speaking?")]
        public string speakerName;
        [Tooltip("What are they saying?")]
        public string dialogue;
        [Tooltip("How long does it take for the each individal leter to print onto the screen?")]
        public float printDuration;
        public AudioClip dialogueVoice;
    }
}
