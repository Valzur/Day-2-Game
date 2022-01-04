using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyProperties
{
    public int initialHealth;
    public float velocity;
    public int hexesDropped;
}

public class Enemy : MonoBehaviour
{
    [SerializeField] EnemyProperties initialProperties;
    protected int currentHealth;
    protected float velocity;

    protected void Setup()
    {
        currentHealth = initialProperties.initialHealth;
        velocity = initialProperties.velocity; 
    }

    public void GetDamage(int damage)
    {
        currentHealth-= damage;
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            GameManager.Instance.AddHexes(initialProperties.hexesDropped);
            GameManager.Instance.RemoveEnemy(this);
            Destroy(gameObject);
        }
    }
}