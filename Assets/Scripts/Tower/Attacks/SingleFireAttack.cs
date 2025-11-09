using UnityEngine;

public class SingleFireTowerAttack : TowerAttack
{
    [SerializeField, Tooltip("Set the bullet that will be fired.")]
    private GameObject bullet = null;
    [SerializeField, Tooltip("Set the game object that rotates towards the target. If null, the game object holding the script will rotate instead.")]
    private Transform canon = null;

    private Transform _target = null; 
    
    protected override void Attack()
    {
        if (bullet == null) return;
        
        // Turn off attacked until the end of this method has been reached.
        attacked = false;

        _target = GetTargetTransform();

        if (_target != null)
        {
            VisualizeAttack();
            GameObject spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            if (spawnedBullet.TryGetComponent(out SingleDirectionMovement sDL))
                sDL.direction = _target.transform.position - transform.position;
            if (spawnedBullet.TryGetComponent(out BulletScript bulletScript))
                bulletScript.Damage = attackDamage;
        }
        else return;
        
        attacked = true;
    }

    protected Transform GetTargetTransform()
    {
        Collider[] enemyColliders = CheckEnemiesInRange();
        
        if (enemyColliders == null || enemyColliders.Length <= 0) return null;

        GameObject enemy = GetFirstEnemy(enemyColliders);
        
        return enemy == null ? null : enemy.transform;
    }

    protected override void VisualizeAttack()
    {
        if (canon != null)
            canon.rotation = Quaternion.LookRotation(_target.transform.position - transform.position);
        else
            transform.rotation = Quaternion.LookRotation(_target.transform.position - transform.position);
    }
}