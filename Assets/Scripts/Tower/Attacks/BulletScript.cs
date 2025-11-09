using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField, Tooltip("How much damage the GameObject does."), Min(0.1f)]
    private float damage = 10;
    [SerializeField, Tooltip("How much time this GameObject is active until it gets destroyed."), Min(0.1f)]
    private float timeUntilDespawn = 3;
    [SerializeField, Tooltip("Set if the GameObject should be destroyed on hit.")]
    private bool destroyOnHit = true;

    private Timer deathTimer;

    private void Awake()
    {
        deathTimer = new Timer(timeUntilDespawn, DestroyBullet);
    }

    // Bullet flies until destroyed
    void Update()
    {
        deathTimer.UpdateTimer();
    }

    private void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>()?.TakeDamage(damage);
            if (destroyOnHit)
                Destroy(gameObject);
        }
    }

    public float Damage
    {
        get { return damage; }
        set
        {
            if (value > 0)
                damage = value;
        }
    }
}