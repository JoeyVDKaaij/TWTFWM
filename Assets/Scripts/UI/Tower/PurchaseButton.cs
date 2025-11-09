using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PurchaseButton : MonoBehaviour
{
    [Header("Purchase options")]
    [SerializeField, Tooltip("Short for Tower Scriptable Object. Set which tower should spawn when pressing the button.")]
    private TowerScriptableObjects tSO = null;
    protected Button button;
    [SerializeField, Tooltip("Image component to show off the tower.")]
    private Image image;
    [SerializeField, Tooltip("Text component to show off the name.")]
    protected TMP_Text nameText;
    [SerializeField, Tooltip("Text  component to show off the costs.")]
    protected TMP_Text costText;

    private TowerPlacement _towerPlacementGuide;

    protected bool lastCheckedMoney = false;
    protected bool lastCheckedState = false;
    
    protected void OnEnable()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Purchase);
        GameManager.onMoneyChanged += OnMoneyChanged;
        GameManager.onGameStateChanged += OnStateChanged;
        
        if (GameManager.instance != null)
        {
            OnMoneyChanged(GameManager.instance.Money);
            OnStateChanged(GameManager.instance.State);
        }
    }

    protected void OnDisable()
    {
        button.onClick.RemoveListener(Purchase);
        GameManager.onMoneyChanged -= OnMoneyChanged;
        GameManager.onGameStateChanged -= OnStateChanged;
    }

    protected void OnDestroy()
    {
        button.onClick.RemoveListener(Purchase);
        GameManager.onMoneyChanged -= OnMoneyChanged;
        GameManager.onGameStateChanged -= OnStateChanged;
    }

    public virtual void Purchase()
    {
        if (tSO != null && _towerPlacementGuide != null)
        {
            _towerPlacementGuide.gameObject.SetActive(true);
            _towerPlacementGuide.SetTower(tSO.towerPrefab, tSO);
        }
    }

    protected virtual void OnMoneyChanged(int pMoney)
    {
        bool tSOExist = tSO != null;
        lastCheckedMoney = tSOExist && tSO.cost <= pMoney;
        button.interactable = lastCheckedMoney && lastCheckedState;
    }

    protected virtual void OnStateChanged(GameState state)
    {
        lastCheckedState = state == GameState.building;
        button.interactable = lastCheckedMoney && lastCheckedState;
    }
    
    public void SetTower(TowerScriptableObjects pTower, TowerPlacement pPlacementGuide)
    {
        tSO = pTower;
        
        // No checks to receive errors in the console.
        // That way we know the prefab has its components not connected properly.
        image.sprite = tSO.icon;
        nameText.SetText(tSO.name);
        costText.SetText(tSO.cost.ToString());
        
        _towerPlacementGuide = pPlacementGuide;
    }
}