using System;
using System.Collections.Generic;
using UnityEngine;

public class DebuffAttack : TowerAttack
{
    [Header("Debuff Settings")]
    [SerializeField]
    private float debuffTime = 4;
    [SerializeField]
    private DebuffType debuffType = DebuffType.MovementSpeed;
    
    // Debuff type will be chosen in the inspector and matched with the appropriate method using in the dictionary
    // allowing for easier customization when changing the tower that much.
    public enum DebuffType
    {
        MovementSpeed
    }

    private Dictionary<DebuffType, Action<GameObject>> DebuffMethod = new Dictionary<DebuffType, Action<GameObject>>();

    private void Start()
    {
        DebuffMethod.Add(DebuffType.MovementSpeed, MovementSpeedDebuff);
    }

    protected override void Attack()
    {
        // Turn off attacked until the end of this method has been reached.
        attacked = false;
        
        Collider[] enemyColliders = CheckEnemiesInRange();
        
        if (enemyColliders == null || enemyColliders.Length <= 0) return;

        foreach (Collider enemyCollider in enemyColliders)
        {
            DebuffMethod[debuffType](enemyCollider.gameObject);
        }

        VisualizeAttack();
        
        attacked = true;
    }

    private void MovementSpeedDebuff(GameObject target)
    {
        if (target.TryGetComponent(out Movement movement))
            movement.SpeedDebuff(attackDamage, debuffTime);
    }

    protected override void VisualizeAttack()
    {
        if (attackParticleSystem == null) return;

        ParticleSystem.MainModule main = attackParticleSystem.main;
        main.startLifetime = CurrentAttackRange / main.startSpeed.constant;
        attackParticleSystem.Play();
    }
}