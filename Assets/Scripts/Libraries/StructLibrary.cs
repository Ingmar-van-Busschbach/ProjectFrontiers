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
}
