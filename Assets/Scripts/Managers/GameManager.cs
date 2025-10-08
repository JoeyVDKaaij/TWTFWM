using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }
    
    [Header("Game Settings Settings")]
    [SerializeField, Tooltip("Set how much HP the player has.")]
    private float health = 100;
    [SerializeField, Tooltip("Set how much money the player has."), Min(0)]
    private float money = 0;
    [SerializeField, Tooltip("Set how fast the game goes."), Min(0)]
    private float gameSpeed = 1f;

    private float fixedDeltaTime;

    private GameState state = GameState.building;

    public static event Action<GameState> onGameStateChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (transform.parent.gameObject != null) DontDestroyOnLoad(transform.parent.gameObject);
            else DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
    
    private void Update()
    {
        if (health <= 0)
        {
            state = GameState.gameOver;
            onGameStateChanged?.Invoke(state);
        }
        
        // Set the game speed depending on the gameSpeed value
        if (Time.timeScale != gameSpeed)
        {
            Time.timeScale = gameSpeed;
            // Adjust fixed delta time according to timescale
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }

    #region Getters

    public GameManager Instance 
    { 
        get { return instance; }
    }

    public float GetHealth
    {
        get { return health; }
    }

    public float GetMoney
    {
        get { return money; }
    }

    #endregion

    #region Functions and Methods


    public void DamagePlayer(float pDamage)
    {
        health -= pDamage;
    }

    public GameState ChangeGameState(GameState pState)
    {
        state = pState;
        onGameStateChanged?.Invoke(state);
        return pState;
    }

    public void ChargePlayer(int pPrice)
    {
        money -= pPrice;
    }

    public void AddMoney(int pMoney)
    {
        money += pMoney;
    }

    #endregion

}
public enum GameState
{
    play,
    building,
    pause,
    gameOver
}