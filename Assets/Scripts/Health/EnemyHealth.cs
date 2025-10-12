using UnityEngine;

public class EnemyHealth : Health
{
    [Header("Enemy Settings")]
    [SerializeField, Tooltip("The amount of gold the enemy possess"), Min(0)]
    private int gold = 1;
    
    protected override void Awake()
    {
        base.Awake();
        if (gold < 0) gold = 0;
    }

    protected override void Death()
    {
        if (GameManager.instance != null)
            GameManager.instance.AddMoney(gold);
        if (WaveManager.instance != null)
            WaveManager.instance.EnemyDied();
        
        Destroy(gameObject);
    }
}