using UnityEngine;
using System;

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Settings")]
    [SerializeField, Tooltip("The amount of damage the enemy deals to the player"), Min(1)]
    private int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndPoint")) 
        {
            if (GameManager.instance?.playerHp != null)
                GameManager.instance.playerHp.TakeDamage(damage);
            
            Destroy(gameObject);
        }
    }
}
