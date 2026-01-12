using UnityEngine;

/// <summary>
/// Utility script that manages the visibility and capture of the mouse cursor.
/// </summary>
public class CursorManager : MonoBehaviour
{
    [SerializeField] private bool showCursor;
    [SerializeField] private bool lockCursor;
    public static CursorManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void OnDestroy()
    {
        Instance = null; // This is technically not needed as on scene loading it should automatically delete the Instance reference, but it is a precaution.
    }
    public void Start()
    {
        ChangeMouseLock(showCursor, lockCursor);
    }
    public void ChangeMouseLock(bool showCursor, bool lockCursor, bool update = false)
    {
        if (update)
        {
            this.showCursor = showCursor;
            this.lockCursor = lockCursor;
        }
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!showCursor)
        {
            Cursor.visible = showCursor;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
