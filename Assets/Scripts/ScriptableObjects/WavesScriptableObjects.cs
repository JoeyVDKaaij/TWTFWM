using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Enemy
{
    /// <param name="enemy">The enemy that spawns.</param>
    public GameObject enemyObj;
   
    /// <param name="spawnCounter">How many time the enemy can spawn this round.</param>
    [Min(1)]
    public int spawnCounter;

    [Min(0)]
    public int chanceOfSpawning;

    public Enemy(GameObject pEnemyObj, int pSpawnCounter, int pChanceOfSpawning)
    {
        enemyObj = pEnemyObj;
        spawnCounter = pSpawnCounter < 1 ? 1 : pSpawnCounter;
        chanceOfSpawning = pChanceOfSpawning;
    }
}

[CreateAssetMenu(fileName = "Wave", menuName = "TowerDefenseAssets/Wave", order = 1)]
public class WavesScriptableObjects : ScriptableObject
{
    [Header("Wave Settings")]
    [SerializeField, Tooltip("Set the rate that enemies spawn in seconds."), Min(1)]
    public float spawnRate = 5.0f;
    [SerializeField, Tooltip("Set how much chance the spawners have to spawn nothing."), Min(0)]
    public int chanceOfNothing = 5;
    
    [Header("Enemies Settings")]
    [Tooltip("Set the enemies and how many of them spawn this wave.")]
    public Enemy[] enemies = null;
}
