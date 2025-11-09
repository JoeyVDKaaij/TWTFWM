using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public struct Enemy
{
    /// <param name="enemy">The enemy that spawns.</param>
    public GameObject enemyObj;
   
    /// <param name="spawnCounter">How many time the enemy can spawn this round.</param>
    [Min(1)]
    public int spawnCounter;

    [Range(1, 100)]
    public int chanceOfSpawning;

    public Enemy(GameObject pEnemyObj, int pSpawnCounter, int pChanceOfSpawning)
    {
        enemyObj = pEnemyObj;
        
        // Ensuring that spawn counter and chances of spawning have at least a value above 0
        spawnCounter = pSpawnCounter < 1 ? 1 : pSpawnCounter;
        chanceOfSpawning = pChanceOfSpawning < 1 ? 1 : pChanceOfSpawning;
    }
}

[CreateAssetMenu(fileName = "Wave", menuName = "TowerDefenseAssets/Wave", order = 1)]
public class WavesScriptableObjects : ScriptableObject
{
    [Header("Wave Settings")]
    [SerializeField, Tooltip("Set the rate that enemies spawn in seconds."), Min(1)]
    public float spawnRate = 5.0f;
    
    [Header("Enemies Settings")]
    [Tooltip("Set the enemies and how many of them spawn this wave.")]
    public Enemy[] enemies = new Enemy[] { new Enemy(null, 1, 100) };

    public Enemy GetRandomEnemy()
    {
        if (enemies.Length == 0) return new Enemy(null, 1, 100);
        
        return enemies[Random.Range(0, enemies.Length)];
    }

    public int GetRandomEnemyIdByChance()
    {
        if (enemies == null || enemies.Length == 0) 
            return -1;

        List<int> ids = new List<int>();
        for (int i = 0; i < enemies.Length; i++) ids.Add(i);
        
        List<int> randomizedEnemyIdList = SortIdListByRandom(ids);
        
        int chance = Random.Range(0, 100);
        int chosenId = -1;
        
        foreach (int id in randomizedEnemyIdList)
        {
            if (enemies[id].chanceOfSpawning >= chance) chosenId = id;
        }
        
        return chosenId;
    }

    private List<int> SortIdListByRandom(List<int> pId)
    {
        if (pId == null || pId.Count == 0) return null;
        
        List<int> settledIdList = new List<int>();
        
        do
        {
            int randomId = Random.Range(0, pId.Count);
            settledIdList.Add(pId[randomId]);
            pId.RemoveAt(randomId);
        } 
        while (pId.Count > 0);
        
        return settledIdList;
    }
}
