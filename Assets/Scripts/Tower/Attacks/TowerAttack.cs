using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class TowerAttack : MonoBehaviour
{
    [Header("Attack Stats")]
    [SerializeField, Min(0)]
    protected float attackRate = 1;
    [SerializeField, Min(0), Tooltip("Set the amount of damage that the tower does or set the value of the debuff.")]
    protected float attackDamage = 10;
    [SerializeField, Min(0)]
    protected float attackRange = 10;

    [Header("Misc")]
    [SerializeField]
    private LayerMask enemyLayerMask = 1 << 3;
    
    private Timer _attackTimer;
    protected bool attacked = false;

    protected float upgradeAttackRate = 0;
    protected float upgradeAttackRange = 0;
    
    protected ParticleSystem attackParticleSystem;

    // CurrentAttackDamage can be improved when the damage is implemented
    public float CurrentAttackDamage
    {
        get { return attackDamage; }
    }
    public float CurrentAttackRate
    {
        get { return attackRate - upgradeAttackRate; }
    }
    public float CurrentAttackRange
    {
        get { return attackRange + upgradeAttackRange; }
    }
    
    private void Awake()
    {
        _attackTimer = new Timer(CurrentAttackRate, Attack);
        attackParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    private void OnValidate()
    {
        if (_attackTimer != null)
            _attackTimer.UpdateDeadline(CurrentAttackRate);
    }

    protected void Update()
    {
        if (!attacked)
        {
            _attackTimer.SetOffTimer();
            return;
        }
        
        _attackTimer.UpdateTimer();
    }

    protected virtual void Attack() {}

    protected Collider[] CheckEnemiesInRange()
    {
        // Add a vertical offset so that everything in range gets detected ignoring y range.
        Vector3 capsuleOffset = new Vector3(0, 10);
        return Physics.OverlapCapsule(transform.transform.position - capsuleOffset, 
            transform.transform.position + capsuleOffset, attackRange + upgradeAttackRange, enemyLayerMask);
    }

    protected GameObject GetFirstEnemy(Collider[] pColliders)
    {
        if (pColliders == null || pColliders.Length <= 0) return null;
        
        // Add a bit more offset to the range to ensure every enemy is accounted for.
        (GameObject enemy, float remainingDistance) leastRemainingDestination = (null, -1);
        foreach (Collider col in pColliders)
        {
            if (col.gameObject.TryGetComponent(out NavMeshAgent agent) && 
                agent.remainingDistance < leastRemainingDestination.remainingDistance || 
                leastRemainingDestination.remainingDistance == -1)
            {
                leastRemainingDestination = (col.gameObject, agent.remainingDistance);
            }
        }
        
        return leastRemainingDestination.enemy;
    }
    
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, attackRange + upgradeAttackRange);
#endif
    }

    public void ApplyUpgrade(Upgrade.UpgradeType type, float value)
    {
        switch (type)
        {
            case Upgrade.UpgradeType.FireRate:
                upgradeAttackRate = value;
                break;
            
            case Upgrade.UpgradeType.Range:
                upgradeAttackRange = value;
                _attackTimer.UpdateDeadline(CurrentAttackRate);
                break;
        }
    }
    
    protected virtual void VisualizeAttack() {}
}