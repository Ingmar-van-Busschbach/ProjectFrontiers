using UnityEditor;
using UnityEngine;

public class QuitScene : MonoBehaviour
{
   public void quittingScene()
    {
        Application.Quit();
        EditorApplication.isPlaying = false;
    }
}
