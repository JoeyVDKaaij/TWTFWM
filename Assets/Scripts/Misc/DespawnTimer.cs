using UnityEngine;

public class DespawnTimer : MonoBehaviour
{
    [Header("Despawn Settings")]
    [SerializeField, Tooltip("How much time this GameObject is active until it gets destroyed."), Min(0)]
    private float timeUntilDespawn = 3;

    private Timer deathTimer;

    private void Awake()
    {
        deathTimer = new Timer(timeUntilDespawn, DestroyObject);
    }

    // Bullet flies until destroyed
    void Update()
    {
        deathTimer.UpdateTimer();
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }
}
