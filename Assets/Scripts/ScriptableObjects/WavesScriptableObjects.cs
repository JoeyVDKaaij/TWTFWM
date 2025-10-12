using System;
using System.Collections.Generic;
using System.Linq;
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

    public Enemy GetRandomEnemyByChance()
    {
        if (enemies.Length == 0) return new Enemy(null, 1, 100);

        List<Enemy> randomizedEnemyList = SortListByRandom();
        
        int chance = Random.Range(0, 100);
        Enemy? chosenEnemy = null;
        
        foreach (Enemy enemy in randomizedEnemyList)
        {
            if (enemy.chanceOfSpawning >= chance) chosenEnemy = enemy;
        }

        if (chosenEnemy == null) chosenEnemy = randomizedEnemyList[0];
        
        return (Enemy)chosenEnemy;
    }

    private List<Enemy> SortListByRandom()
    {
        if (enemies.Length == 0) return null;
        
        List<Enemy> pendingEnemies = enemies.ToList();
        List<Enemy> settledEnemies = new List<Enemy>();
        
        do
        {
            int enemyId = Random.Range(0, pendingEnemies.Count);
            settledEnemies.Add(pendingEnemies[enemyId]);
            pendingEnemies.RemoveAt(enemyId);
        } 
        while (pendingEnemies.Count > 0);
        
        return settledEnemies;
    }

    // Find ID of enemy in enemy array.
    // TODO: Might be an expensive implementation. Consult QA
    public int GetEnemyIdByEnemy(Enemy pEnemy)
    {
        return Array.IndexOf(enemies, pEnemy);
    }
}
