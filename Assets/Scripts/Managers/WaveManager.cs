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
    
    private List<SpawnScript> _spawners = new List<SpawnScript>();

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
            
            // Currently not really needed since it is only really needed for one scene!
            // Maybe in future updates!
            // TODO: Look into this!
            // if (transform.parent.gameObject != null) DontDestroyOnLoad(transform.parent.gameObject);
            // else DontDestroyOnLoad(gameObject);
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
        _enemyTimer = new Timer(_currentWave.spawnRate, UpdateEnemyPhase);
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
        Enemy enemy = _currentWave.GetRandomEnemyByChance();
        
        if (enemy.enemyObj == null)
        {
            Debug.LogError("Wave has no enemies to spawn.");
            return;
        }
        
        _spawners[Random.Range(0,_spawners.Count)].SpawnGameObject(enemy.enemyObj);
        _enemyIdSpawnCounter[_currentWave.GetEnemyIdByEnemy(enemy)]--;
        _enemiesAlive++;

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

    private void ChangeToEnemyPhase()
    {
        _state = GameState.play;
        if (GameManager.instance != null)
            GameManager.instance.ChangeGameState(_state);
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

    public void AddSpawner(SpawnScript spawner)
    {
        _spawners.Add(spawner);
    }

    public void RemoveSpawner(SpawnScript spawner)
    {
        _spawners.Remove(spawner);
    }

    private void ChangeToGameOver()
    {
        // TODO: Finish the game or SOMETHING
        _state = GameState.gameOver;
        
        Debug.Log($"Game Over! State: {_state}");
        
        if (GameManager.instance != null) GameManager.instance.ChangeGameState(_state);
    }

    public void EnemyDied()
    {
        _enemiesAlive--;
    }
    
    #endregion

    # region Other Functions, Methods and Events

    private void ChangeGameState(GameState pState)
    {
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