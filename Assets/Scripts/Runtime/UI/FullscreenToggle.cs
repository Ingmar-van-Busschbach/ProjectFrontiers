using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class FullscreenToggle : MonoBehaviour
{
    private Toggle fullscreenToggle;

    private void Start()
    {
        fullscreenToggle = GetComponent<Toggle>();
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void ApplyFullscreen()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
    }
}
