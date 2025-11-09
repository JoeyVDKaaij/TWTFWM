using UnityEngine;

public class AOEAttack : TowerAttack
{
    protected override void Attack()
    {
        // Turn off attacked until the end of this method has been reached.
        attacked = false;
        
        Collider[] enemyColliders = CheckEnemiesInRange();
        
        if (enemyColliders == null || enemyColliders.Length <= 0) return;

        foreach (Collider enemyCollider in enemyColliders)
        {
            if (enemyCollider.TryGetComponent(out EnemyHealth enemyHealth))
                enemyHealth.TakeDamage(attackDamage);
        }

        VisualizeAttack();
        
        attacked = true;
    }

    protected override void VisualizeAttack()
    {
        if (attackParticleSystem == null) return;

        ParticleSystem.MainModule main = attackParticleSystem.main;
        main.startLifetime = CurrentAttackRange / main.startSpeed.constant;
        attackParticleSystem.Play();
    }
}