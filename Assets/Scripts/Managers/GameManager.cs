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
    private float money = 0;
    [SerializeField, Tooltip("Set how fast the game goes."), Min(0)]
    private float gameSpeed = 1f;

    private float fixedDeltaTime;

    public GameState state { get; private set; } = GameState.building;

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
        
        playerHp = GetComponent<PlayerHealth>();

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
        // Set the game speed depending on the gameSpeed value
        if (Time.timeScale != gameSpeed)
        {
            Time.timeScale = gameSpeed;
            // Adjust fixed delta time according to timescale
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
        }
    }

    #region Getters

    public float GetMoney
    {
        get { return money; }
    }

    #endregion

    #region Functions and Methods

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