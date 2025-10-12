using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField, Tooltip("How much damage the GameObject does."), Min(0.1f)]
    private int damage = 10;
    [SerializeField, Tooltip("How much time this GameObject is active until it gets destroyed."), Min(0.1f)]
    private float timeUntilDespawn = 3;
    [SerializeField, Tooltip("Set if the GameObject should be destroyed on hit.")]
    private bool destroyOnHit = true;
    

    private float timer = 0;

    // Bullet flies until destroyed
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= timeUntilDespawn)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().TakeDamage(damage);
            if (destroyOnHit)
                Destroy(gameObject);
        }
    }

    public int GetDamage
    {
        get { return damage; }
    }
}