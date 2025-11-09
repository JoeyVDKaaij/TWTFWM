using System;
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
    
    private List<EnemySpawnScript> _spawners = new List<EnemySpawnScript>();

    private int _spawnCount;
    private int _waveId = 0;
    private Timer _buildTimer;
    private Timer _enemyTimer;

    private GameState _state = GameState.building;

    // Starting at -1 to prevent extra lines in the SetUpWave method
    private int _currentWaveId = -1;
    private WavesScriptableObjects _currentWave = null;
    private Dictionary<int, int> _enemyIdSpawnCounter =  new Dictionary<int, int>();
    private int _enemiesAlive = 0;
    
    public event Action<int> onWaveChanged;
    
    #region UnityFunctions
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        GameManager.onGameStateChanged += ChangeGameState;
    }

    private void Start()
    {
        SetupNextWave();

        _buildTimer = new Timer(timeForBuildingPhase, ChangeToEnemyPhase);
        if (_currentWave != null)
            _enemyTimer = new Timer(_currentWave.spawnRate, UpdateEnemyPhase);
        
        if (waves == null || _spawners == null)
        {
            Debug.LogError("There is no waves or spawners assigned!");
        }
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
        switch (_state)
        {
            case GameState.play:
                _enemyTimer.UpdateTimer();
                break;
            case GameState.building:
                _buildTimer.UpdateTimer();
                break;
        }
    }

    private void UpdateEnemyPhase()
    {
        HandleEnemySpawning();

        // Check if every enemy has spawned in the current wave
        // Go to the next round if true
        bool readyForWave = _enemyIdSpawnCounter.All(x => x.Value <= 0) &&
                            _enemiesAlive <= 0;
        
        if (readyForWave)
        {
            _state = GameState.building;
            if (GameManager.instance != null)
                GameManager.instance.ChangeGameState(_state);

            SetupNextWave();
        }
    }

    private void HandleEnemySpawning()
    {
        if (_spawners.Count <= 0) return;
        
        int enemyId = _currentWave.GetRandomEnemyIdByChance();
        if (enemyId < 0 || _enemyIdSpawnCounter[enemyId] <= 0) return;
        
        Enemy currentEnemy = _currentWave.enemies[enemyId];
        if (currentEnemy.enemyObj == null)
        {
            Debug.LogError("Wave has no enemies to spawn.");
            return;
        }
        
        int randomSpawnerId = Random.Range(0, _spawners.Count);
        _spawners[randomSpawnerId].SpawnGameObject(currentEnemy.enemyObj);
        
        if (_enemyIdSpawnCounter.ContainsKey(enemyId))
            _enemyIdSpawnCounter[enemyId]--;
        
        _enemiesAlive++;
    }

    private void ChangeToEnemyPhase()
    {
        if (GameManager.instance != null) 
            _state = GameManager.instance.ChangeGameState(GameState.play);
        else _state = GameState.play;
    }

    private void SetupNextWave()
    {
        _currentWaveId++;
        if (_currentWaveId >= waves.Length)
        {
            ChangeToGameOver();
            return;
        }
        
        _currentWave = waves[_currentWaveId];

        _enemyIdSpawnCounter.Clear();
        for (int i = 0; i < _currentWave.enemies.Length; i++)
        {
            _enemyIdSpawnCounter.Add(i, _currentWave.enemies[i].spawnCounter);
        }
        
        if (_enemyIdSpawnCounter.Count == 0) SetupNextWave();
        
        onWaveChanged?.Invoke(_currentWaveId+1);
    }

    public void AddSpawner(EnemySpawnScript spawner)
    {
        _spawners.Add(spawner);
    }

    public void RemoveSpawner(EnemySpawnScript spawner)
    {
        _spawners.Remove(spawner);
    }

    private void ChangeToGameOver()
    {
        _state = GameState.gameOver;
        
        if (GameManager.instance != null)
        {
            GameManager.instance.ChangeGameState(_state);
            GameManager.instance.resultScreen(true);
        }
    }

    public void EnemyDied()
    {
        _enemiesAlive--;
        Debug.Log($"Enemy died. Counting: {_enemiesAlive}");
    }
    
    #endregion

    # region Other Functions, Methods and Events

    private void ChangeGameState(GameState pState)
    {
        Debug.Log($"State changed to {pState}");
        _state = pState;
    }

    #endregion
    
    #region Getters

    public int GetWave
    {
        get { return _waveId + 1; }
    }

    public int GetAmountOfWaves
    {
        get { return waves.Length; }
    }

    public int GetBuildingPhaseTimer
    {
        get { return Mathf.RoundToInt(timeForBuildingPhase - _buildTimer.Time); }
    }
    
    #endregion
}