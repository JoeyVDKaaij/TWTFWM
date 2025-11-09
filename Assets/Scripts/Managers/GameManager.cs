using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    public Health playerHp
    {
        get;
        private set;
    }
    
    [Header("Game Settings Settings")]
    [SerializeField, Tooltip("Set how much money the player has."), Min(0)]
    private int money = 0;
    [SerializeField, Tooltip("Set how fast the game goes."), Min(0)]
    private float gameSpeed = 1f;

    private float fixedDeltaTime;

    [SerializeField, Tooltip("Set the current state of the game.")]
    private GameState state = GameState.building;
    [SerializeField, Tooltip("Set how fast the game goes.")]
    private GameObject winScreenPrefab = null;
    [SerializeField, Tooltip("Set how fast the game goes.")]
    private GameObject loseScreenPrefab = null;

    public static event Action<int> onMoneyChanged;
    
    public int Money
    {
        get
        {
            return money;
        }
        private set
        {
            money = value;

            if (money < 0) money = 0;
            
            onMoneyChanged?.Invoke(money);
        }
    }


    public GameState State
    {
        get { return state; }
        private set
        {
            state = value;
        }
    }

    public static event Action<GameState> onGameStateChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        playerHp = GetComponent<PlayerHealth>();

        this.fixedDeltaTime = Time.fixedDeltaTime;
    }

    private void Start()
    {
        onMoneyChanged?.Invoke(money);
        onGameStateChanged?.Invoke(state);
        if (playerHp != null) playerHp.InvokeOnHealthChanged();
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
        // Set the game speed depending on the gameSpeed value
        if (Time.timeScale != gameSpeed)
        {
            Time.timeScale = gameSpeed;
            // Adjust fixed delta time according to timescale
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }

    public void ChangeGameSpeed(float pSpeed)
    {
        if (pSpeed >= 0)
            gameSpeed = pSpeed;
    }

    #region Functions and Methods

    public GameState ChangeGameState(GameState pState)
    {
        state = pState;
        onGameStateChanged?.Invoke(state);
        return pState;
    }

    public void ChargePlayer(int pPrice)
    {
        Money -= pPrice;
    }

    public void AddMoney(int pMoney)
    {
        Money += pMoney;
    }

    public void resultScreen(bool wonGame)
    {
        if (winScreenPrefab == null)
        {
            if (loseScreenPrefab != null)
            {
                Instantiate(loseScreenPrefab);
            }
            
            return;
        }
        
        if (loseScreenPrefab == null)
        {
            Instantiate(winScreenPrefab);
            return;
        }
        
        Instantiate(wonGame ? winScreenPrefab : loseScreenPrefab);
    }

    #endregion

}
public enum GameState
{
    play,
    building,
    gameOver
}