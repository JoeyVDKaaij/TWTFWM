using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class TowerShopSetupScript : MonoBehaviour
{
    [Header("Shop Settings")] 
    [SerializeField, Tooltip("Set the towers for sale.")]
    private GameObject[] towersForPurchase = null;

    private GridLayoutGroup gridLayoutGroup = null;
    private RectTransform rt = null;
    
    private float currentChildCountSize = 0;
    private float currentTowersForPurchaseSize = 0;
    
    void Start()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        rt = GetComponent<RectTransform>();
        
        SetupShop();
    }

    // private void Update()
    // {
    //     if (currentChildCountSize != rt.childCount && towersForPurchase.Length == 0 || towersForPurchase != null && currentTowersForPurchaseSize != towersForPurchase.Length)
    //     {
    //         SetupShop();
    //     }
    // }

    private void SetupShop()
    {
        if (towersForPurchase != null && towersForPurchase.Length > 0)
        {
            if (rt.childCount > 0)
            {
                foreach (GameObject towerOption in towersForPurchase)
                {
                    Instantiate(towerOption, transform);
                }
            }
            currentTowersForPurchaseSize = towersForPurchase.Length;
        }
        else if (rt.childCount > 0)
        {
            float lowestYPosition = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (lowestYPosition > transform.GetChild(i).transform.localPosition.y)
                    lowestYPosition = transform.GetChild(i).transform.localPosition.y;
            }
            Rect rect = rt.rect;
            rect.height = -lowestYPosition + gridLayoutGroup.padding.top * 2;
            rt.sizeDelta = new Vector2(rect.width, rect.height);
            currentChildCountSize = transform.childCount;
        }
    }
}
