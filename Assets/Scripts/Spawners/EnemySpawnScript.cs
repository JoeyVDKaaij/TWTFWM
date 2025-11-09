using UnityEngine;

public class EnemySpawnScript : SpawnScript
{
    [SerializeField, Tooltip("Set the target for the spawned enemies.")]
    private Transform target;
    
    protected void OnEnable()
    {
        if (WaveManager.instance != null)
        {
            Debug.Log("adding Spawner");
            WaveManager.instance.AddSpawner(this);
        }
    }

    private void Start()
    {
        if (WaveManager.instance != null)
        {
            Debug.Log("adding Spawner");
            WaveManager.instance.AddSpawner(this);
        }
    }

    protected void OnDisable()
    {
        if (WaveManager.instance != null)
            WaveManager.instance.RemoveSpawner(this);
    }
    
    public override GameObject SpawnGameObject(GameObject pObj)
    {
        GameObject obj = base.SpawnGameObject(pObj);

        if (target != null && obj.TryGetComponent(out PathFindingMovement pathFindingMovement))
        {
            pathFindingMovement.SetTarget(target);
        }
        
        return obj;
    }
}