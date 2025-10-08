using UnityEngine;
using System;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField, Tooltip("The amount of gold the enemy possess"), Min(0)]
    private int gold = 1;
    [SerializeField, Tooltip("The amount of health the enemy has"), Min(1)]
    private int health = 1;
    [SerializeField, Tooltip("The amount of damage the enemy deals to the player"), Min(1)]
    private int damage = 1;

    private int maxHealth;
    
    public event Action<float> OnHPChanged;
    private void Awake()
    {
        maxHealth = health;
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
            GameManager.instance.AddMoney(gold);
    }

    public void GainDamage(int pDamage)
    {
        health -= pDamage;
        Debug.Log(health);
        if (health <= 0)
            Destroy(gameObject);
        else
            OnHPChanged?.Invoke(pDamage);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndPoint")) 
        {
            GameManager.instance.DamagePlayer(damage);
            Destroy(gameObject);
        }
    }

    public float GetHealth
    {
        get { return health; }
    }

    public float GetMaxHealth
    {
        get { return maxHealth; }
    }
}
