using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int enemiesFollowing;
    private void Awake()
    {
        if (instance  != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    private void OnDestroy()
    {
        Destroy(this);
    }

    public void UpdateFollowing(int add)
    {
        enemiesFollowing += add;
        Mathf.Clamp(enemiesFollowing, 0, 5);
    }
}
