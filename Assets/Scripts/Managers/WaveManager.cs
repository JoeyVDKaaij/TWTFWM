using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance { get; private set; }

    [Header("Wave Settings")]
    [SerializeField, Tooltip("Set which waves in order the game will play through.")]
    private WavesScriptableObjects[] waves = null;
    [SerializeField, Tooltip("Set how long the building phase last.")]
    private float timeForBuildingPhase = 10;

    [HideInInspector] 
    private List<SpawnScript> _spawners = new List<SpawnScript>();

    private int spawnCount;
    private int waveId = 0;
    private Timer buildTimer;
    private Timer enemyTimer;

    private GameState state = GameState.building;

    // Starting at -1 to prevent extra lines in the SetUpWave method
    private int _currentWaveId = -1;
    private WavesScriptableObjects _currentWave = null;
    private List<int> _enemySpawnCounter =  new List<int>();
    
    #region UnityFunctions
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            // Currently not really needed since it is only really needed for one scene!
            // Maybe in future updates!
            // TODO: Look into this!
            // if (transform.parent.gameObject != null) DontDestroyOnLoad(transform.parent.gameObject);
            // else DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameManager.onGameStateChanged += ChangeGameState;
    }

    private void Start()
    {
        SetupNextWave();

        buildTimer = new Timer(timeForBuildingPhase, ChangeToEnemyPhase);
        enemyTimer = new Timer(_currentWave.spawnRate, UpdateEnemyPhase);
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
        
        GameManager.onGameStateChanged -= ChangeGameState;
    }
    
    #endregion

    #region Spawning

    void Update()
    {
        if (waves == null && _spawners == null)
        {
            Debug.LogError("There is no waves or spawners assigned!");
            return;
        }
        
        if (state == GameState.play)
        {
            enemyTimer.UpdateTimer();
        }
        else if (state == GameState.building)
        {
            buildTimer.UpdateTimer();
        }
    }

    private void UpdateEnemyPhase()
    {
        int enemyId = getIdByChance();
        if (enemyId != -1 && _currentWave.enemies[enemyId].enemyObj != null)
        {
            _spawners[Random.Range(0,_spawners.Count)].SpawnGameObject(_currentWave.enemies[enemyId].enemyObj);
            _enemySpawnCounter[enemyId]--;
        }

        // Check if every enemy has spawned in the current wave
        // Go to the next round if true
        bool readyForWave = _enemySpawnCounter.All(x => x <= 0);
        
        if (readyForWave)
        {
            if (waveId + 1 >= waves.Length)
                waveId = -1;
            
            SetupNextWave();
            
            state = GameManager.instance.ChangeGameState(GameState.building);
        }
    }

    private void ChangeToEnemyPhase()
    {
        state = GameManager.instance.ChangeGameState(GameState.play);
    }

    private int getIdByChance()
    {
        Enemy[] enemies = _currentWave.enemies;
        
        int summedUpChances = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].spawnCounter > 0)
                summedUpChances += enemies[i].chanceOfSpawning;
        }
        summedUpChances += _currentWave.chanceOfNothing;
        float currentChance = summedUpChances / 100 * Random.Range(0,101);
        summedUpChances = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].spawnCounter > 0 && enemies[i].chanceOfSpawning > 0)
            {
                summedUpChances += enemies[i].chanceOfSpawning;
                if (summedUpChances >= currentChance)
                    return i;
            }
        }
        return -1;
    }

    private void SetupNextWave()
    {
        _currentWaveId++;
        if (_currentWaveId >= waves.Length) return;
        
        _currentWave = waves[_currentWaveId];

        _enemySpawnCounter.Clear();
        foreach (Enemy enemy in _currentWave.enemies)
        {
            _enemySpawnCounter.Add(enemy.spawnCounter);
        }
        
        if (_enemySpawnCounter.Count == 0) SetupNextWave();
    }

    public void AddSpawner(SpawnScript spawner)
    {
        _spawners.Add(spawner);
    }

    public void RemoveSpawner(SpawnScript spawner)
    {
        _spawners.Remove(spawner);
    }
    
    #endregion

    # region Other Functions, Methods and Events

    private void ChangeGameState(GameState pState)
    {
        state = pState;
    }

    #endregion
    
    #region Getters

    public WaveManager Instance
    {
        get { return instance; }
    }

    public int GetWave
    {
        get { return waveId + 1; }
    }

    public int GetAmountOfWaves
    {
        get { return waves.Length; }
    }

    public int GetBuildingPhaseTimer
    {
        get { return Mathf.RoundToInt(timeForBuildingPhase - buildTimer.Time); }
    }
    
    #endregion

}