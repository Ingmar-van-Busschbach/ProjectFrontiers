using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TMP_Dropdown))]
public class GraphicsQualitySwitch : MonoBehaviour
{
    [SerializeField] private bool expensiveChanges;

    public void GraphicsSettingChange(int index)
    {
        QualitySettings.SetQualityLevel(index, expensiveChanges);
    }
}
