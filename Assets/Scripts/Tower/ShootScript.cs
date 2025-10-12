using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ShootScript : MonoBehaviour
{
    [Header("Shoot Settings")] 
    [SerializeField, Tooltip("Set how the GameObject shoots.")]
    private FireMode fireMode = FireMode.single;
    [SerializeField, Tooltip("Set the fire rate in seconds."), Min(0)]
    private float fireRate = 3;
    [SerializeField, Tooltip("Set the range."), Min(1)]
    private float range = 3;
    
    [Header("Single Target Settings")] 
    [SerializeField, Tooltip("Set the bullet that the GameObject fires.")]
    private GameObject bullet = null;
    
    [Header("AOE Settings")] 
    [SerializeField, Tooltip("Set the damage all enemies in range receive."), Min(1)]
    private int aoeDamage = 1;
    [SerializeField, Tooltip("Set the GameObject that acts like a AOE effect.")]
    private GameObject aoeGameObject = null;
    
    [Header("Debuf Settings")] 
    [SerializeField, Tooltip("Set how much speed gets decreased."), Min(0.1f)]
    private float slowSpeed = 1;
    [SerializeField, Tooltip("Set how long the speed gets decreased."), Min(0.1f)]
    private float debufTimer = 1;
    [SerializeField, Tooltip("If true, the speed of the affected GameObject is the same as the set amount in the slowSpeed variable.")]
    private bool absoluteSlowSpeed = false;
    [SerializeField, Tooltip("Set the GameObject that acts like a debuf effect.")]
    private GameObject debufGameObject = null;

    private Quaternion lookAtTarget;
    private float timer;
    
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Handles.DrawWireDisc(transform.position, transform.up, range);
#endif
    }

    private void Start()
    {
        if (debufGameObject != null)
        {
            GameObject tempDebufVariable = Instantiate(debufGameObject, transform);
            tempDebufVariable.transform.localScale = new Vector3(range, range, range);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        Vector3 capsuleOffset = new Vector3(0, 1);
        Collider[] colliders = Physics.OverlapCapsule(transform.transform.position - capsuleOffset, transform.transform.position + capsuleOffset, range);
        if (colliders != null)
        {
            switch (fireMode)
            {
                case FireMode.single:
                    SingleShot(colliders);
                    break;
                case FireMode.aoe:
                    AOEAttack(colliders);
                    break;
                case FireMode.debuf:
                    Debuf(colliders);
                    break;
            }
        }
    }

    private void SingleShot(Collider[] pColliders)
    {
        int lowestSiblingIndex = -1;
        for (int i = 0; i < pColliders.Length; i++)
        {
            NavMeshAgent colliderNMA = pColliders[i].GetComponent<NavMeshAgent>();
            if (lowestSiblingIndex == -1 && pColliders[i].gameObject.CompareTag("Enemy"))
                lowestSiblingIndex = i;
            else if (lowestSiblingIndex != -1 && colliderNMA != null && pColliders[i].gameObject.CompareTag("Enemy"))
            {
                if (colliderNMA.remainingDistance <
                    pColliders[lowestSiblingIndex].GetComponent<NavMeshAgent>().remainingDistance)
                {
                    lowestSiblingIndex = i;
                }
            }
        }

        if (lowestSiblingIndex != -1)
        {
            lookAtTarget = LookAtTarget(pColliders[lowestSiblingIndex].transform);
            transform.rotation = Quaternion.Euler(0, lookAtTarget.eulerAngles.y, 0);
            if (timer >= fireRate)
            {
                if (bullet != null)
                {
                    Instantiate(bullet, transform.position, lookAtTarget);
                }
                timer = 0;
            }
        }
    }

    private void AOEAttack(Collider[] pColliders)
    {
        if (timer >= fireRate)
        {
            bool hitEnemy = false;
            foreach (Collider pCollider in pColliders)
            {
                if (pCollider.CompareTag("Enemy"))
                {
                    // pCollider.GetComponent<EnemyScript>().GainDamage(aoeDamage);
                    hitEnemy = true;
                }
            }

            if (hitEnemy)
            {
                if (aoeGameObject != null)
                    Instantiate(aoeGameObject, transform);
                timer = 0;
            }
        }
    }

    private void Debuf(Collider[] pColliders)
    {
        if (timer >= fireRate)
        {
            bool hitEnemy = false;
            foreach (Collider pCollider in pColliders)
            {
                if (pCollider.CompareTag("Enemy"))
                {
                    pCollider.GetComponent<MovementScript>().SlowDownSpeed(slowSpeed, debufTimer, absoluteSlowSpeed);
                    hitEnemy = true;
                }
            }

            if (hitEnemy)
            {
                if (debufGameObject != null)
                    Instantiate(debufGameObject, transform);
                timer = 0;
            }
        }
    }
    
    public void SetValues(TowerScriptableObjects pTower)
    {
        fireMode = pTower.fireMode;
        fireRate = pTower.fireRate;
        range = pTower.range;
        if (pTower.bullet != null) bullet = pTower.bullet;
    }

    private Quaternion LookAtTarget(Transform target, Transform original = null)
    {
        Vector3 directionToTarget;
        
        if (original != null) directionToTarget = target.position - original.position;
        else directionToTarget = target.position - transform.position;
        
        Quaternion rotationToTarget = Quaternion.LookRotation(directionToTarget);
        
        return rotationToTarget;
    }


    public int GetDamage()
    {
        switch (fireMode)
        {
            case FireMode.single:
                return bullet.GetComponent<BulletScript>().GetDamage;
            case FireMode.aoe:
                return aoeDamage;
            default:
                return 0;
        }
    }

    public void ImproveRange(float pAddedRange)
    {
        range = pAddedRange;
    }

    public void ImproveFireRate(float pAddedFireRate)
    {
        fireRate = pAddedFireRate;
    }
    
    public float GetRange
    {
        get { return range; }
    }

    public float GetFireRate
    {
        get { return fireRate; }
    }
    public FireMode GetAttackType
    {
        get { return fireMode; }
    }
}

public enum FireMode
{
    single,
    aoe,
    debuf
}