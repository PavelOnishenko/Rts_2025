using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    int currentHealth;

    void Start() => currentHealth = maxHealth;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
            Destroy(gameObject);
    }
}
