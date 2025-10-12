using UnityEngine;
using TMPro;
using System;

public class GuiManager : MonoBehaviour
{
    [Header("GUI Settings")]
    [SerializeField, Tooltip("The wave text.")]
    private TMP_Text healthText = null;
    [SerializeField, Tooltip("The wave text.")]
    private TMP_Text waveText = null;
    [SerializeField, Tooltip("The money text.")]
    private TMP_Text moneyText = null;
    [SerializeField, Tooltip("The time text.")]
    private TMP_Text timeText = null;
    [SerializeField, Tooltip("The tower shop.")]
    private GameObject towerShop = null;
    [SerializeField, Tooltip("The upgrade shop.")]
    private GameObject upgradeShop = null;
    
    [SerializeField, Tooltip("The time text.")]
    private bool showAmountOfWaves = false;
    public static GuiManager instance { get; private set; }

    private GameState state = GameState.building;

    public static event Action<bool, GameObject> ChangeTowerUI;
    
    #region UnityFunctions
    
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

        GameManager.onGameStateChanged += ChangeGameState;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
        
        GameManager.onGameStateChanged -= ChangeGameState;
    }

    private void Update()
    {
        ChangeTextUI();
    }

    #endregion

    #region Functions, Methods and Events

    private void ChangeTextUI()
    {
        if (GameManager.instance != null) return;
        
        healthText.SetText("Health: " + GameManager.instance?.playerHp?.CurrentHp);
        moneyText.SetText("Money: $" + GameManager.instance.GetMoney);
        timeText.SetText("Time left until next round: " + WaveManager.instance.GetBuildingPhaseTimer);
    }

    public void ChangeTowerMenu(bool pTowerSelected, GameObject pTower = null)
    {
        ChangeTowerUI?.Invoke(pTowerSelected, pTower);
    }
    
    public void PurchaseUpgrade()
    {
        
    }

    private void ChangeGameState(GameState pState)
    {
        state = pState;
        timeText.gameObject.SetActive(pState == GameState.building);
    }

    #endregion
    
    #region Getters

    public GuiManager Instance
    {
        get { return instance; }
    }
    
    #endregion
}