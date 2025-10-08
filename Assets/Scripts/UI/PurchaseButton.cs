using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    [Header("Purchase options")]
    [SerializeField, Tooltip("Set which tower spawn when pressing the button.")]
    private TowerScriptableObjects towerScriptableObjects = null;
    private Button button;
    
    void Start()
    {
        button = GetComponent<Button>();
    }
    
    void Update()
    {
        if (towerScriptableObjects != null && button != null)
        {
            if (towerScriptableObjects.price > GameManager.instance.GetMoney)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
            }
        }
        else
        {
            Debug.LogWarning("TowerScriptableObject is not set! This button won't do anything");
            if (button != null)
            {
                button.interactable = false;
            }
            else
            {
                Debug.LogWarning("Button component is not set! This button won't do anything");
            }
        }

    }

    public void Purchase()
    {
        if (towerScriptableObjects != null)
        {
            TowerManager.instance.EnableTowerPlacement(towerScriptableObjects);
        }
    }
}
