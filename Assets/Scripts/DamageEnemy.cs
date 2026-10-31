using UnityEngine;

public class DamageEnemy : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    float currentHealth;
    void Start()
    {
        currentHealth = maxHealth;
    }
    void FixedUpdate()
    {
        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void TakeHit(float amount)
    {
        currentHealth = Mathf.Max(0, currentHealth - amount);
    }
}
