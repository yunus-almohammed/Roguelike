using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject enemyData;

    [Header("XP Drop")]
    public GameObject xpGemPrefab;
    public int xpAmount = 1;

    // Current stats
    float currentMoveSpeed;
    float currentHealth;
    float currentDamage;

    void Awake()
    {
        // Assign the variables
        currentMoveSpeed = enemyData.MoveSpeed;
        currentHealth = enemyData.MaxHealth;
        currentDamage = enemyData.Damage;
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        DropXP();
        Destroy(gameObject);
    }

    private void DropXP()
    {
        if (xpGemPrefab == null)
            return;

        GameObject xpGem = Instantiate(xpGemPrefab, transform.position, Quaternion.identity);

        ExperienceGem experienceGem = xpGem.GetComponent<ExperienceGem>();

        if (experienceGem != null)
        {
            experienceGem.xpAmount = xpAmount;
        }
    }
}