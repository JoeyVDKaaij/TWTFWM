using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeShopSetupScript : MonoBehaviour
{
    [Header("Shop Settings")] 
    [SerializeField, Tooltip("Set the icon that updates to the corrosponding tower.")]
    private Image icon = null;
    [SerializeField, Tooltip("Set the text that updates the damage.")]
    private TMP_Text damage = null;
    [SerializeField, Tooltip("Set the text that updates the range.")]
    private TMP_Text range = null;
    [SerializeField, Tooltip("Set the text that updates the attack interval.")]
    private TMP_Text attackInterval = null;
    [SerializeField, Tooltip("Set the text that updates the attack type.")]
    private TMP_Text attackType = null;
    [SerializeField, Tooltip("Set the GameObject where the user purchase the tower upgrade.")]
    private GameObject rangeUpgrade = null;
    [SerializeField, Tooltip("Set the GameObject where the user purchase the tower upgrade.")]
    private GameObject fireRateUpgrade = null;

    private GridLayoutGroup gridLayoutGroup = null;
    private RectTransform rt = null;
    
    private float currentChildCountSize = 0;
    private float currentTowersForPurchaseSize = 0;
    
    void Start()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        rt = GetComponent<RectTransform>();
        
        GuiManager.ChangeTowerUI += SetupShop;
    }

    private void OnDestroy()
    {
        GuiManager.ChangeTowerUI -= SetupShop;
    }

    // private void Update()
    // {
    //     if (currentChildCountSize != rt.childCount && towersForPurchase.Length == 0 || towersForPurchase != null && currentTowersForPurchaseSize != towersForPurchase.Length)
    //     {
    //         SetupShop();
    //     }
    // }

    private void SetupShop(bool pSelectedTower, GameObject pTower)
    {
        if (pSelectedTower)
        {
            damage.SetText(pTower.GetComponent<ShootScript>().GetDamage().ToString());
            range.SetText(pTower.GetComponent<ShootScript>().GetRange.ToString());
            attackInterval.SetText(pTower.GetComponent<ShootScript>().GetFireRate.ToString());
            attackType.SetText(pTower.GetComponent<ShootScript>().GetAttackType.ToString());
            
            rangeUpgrade.GetComponent<UpgradeButtonScript>().SetUpButton(pTower);
            fireRateUpgrade.GetComponent<UpgradeButtonScript>().SetUpButton(pTower);
        }
    }
}
