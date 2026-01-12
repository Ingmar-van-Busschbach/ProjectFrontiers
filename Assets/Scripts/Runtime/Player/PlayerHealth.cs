using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    protected override void OnDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    protected override void OnDamaged(float damage)
    {
        
    }
}
