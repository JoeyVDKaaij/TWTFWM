using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonScript : MonoBehaviour
{
    private TowerScriptableObjects _towerScriptableObjects;
    private ShootScript _towerShootScript;

    private int _currentCost;

    private bool _upgraded = false;

    private Button _button;
    
    // rangeUpgrade is right now a boolean due to there only being two upgrades.
    // Can be changed to a enum later if more upgradeable values get implemented.
    [SerializeField]
    private bool rangeUpgrade = false;

    private void Start()
    {
        _button = GetComponent<Button>();
    }

    private void Update()
    {
        _button.interactable = _currentCost > GameManager.instance.GetMoney;
    }

    public void SetUpButton(GameObject pTower)
    {
        _towerScriptableObjects = pTower.GetComponent<SelectTowerScript>().towerScriptableObjects;
        _towerShootScript = pTower.GetComponent<ShootScript>();
        
        if (rangeUpgrade)
        {
            for (int i = 0; i < _towerScriptableObjects.rangeUpgrades.Length; i++)
            {
                if (_towerShootScript.GetRange < _towerScriptableObjects.rangeUpgrades[i].rangeUpgrade)
                {
                    _currentCost = _towerScriptableObjects.fireRateUpgrades[i].cost;
                    SetUpText((int)_towerScriptableObjects.rangeUpgrades[i].rangeUpgrade, _currentCost);
                }
            }

            if (_towerScriptableObjects.rangeUpgrades[_towerScriptableObjects.rangeUpgrades.Length - 1].rangeUpgrade <=
                _towerShootScript.GetRange)
            {
                _button.interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < _towerScriptableObjects.fireRateUpgrades.Length; i++)
            {
                if (_towerShootScript.GetRange < _towerScriptableObjects.fireRateUpgrades[i].fireRateUpgrade)
                {
                    _currentCost = _towerScriptableObjects.fireRateUpgrades[i].cost;
                    SetUpText((int)_towerScriptableObjects.fireRateUpgrades[i].fireRateUpgrade, _currentCost);
                }
            }

            if (_towerScriptableObjects.fireRateUpgrades[_towerScriptableObjects.fireRateUpgrades.Length - 1].fireRateUpgrade <=
                _towerShootScript.GetFireRate)
            {
                _button.interactable = false;
            }
        }
    }

    private void SetUpText(int pUpgrade, int pCost)
    {
        transform.GetChild(1).GetComponent<TMP_Text>().SetText("+ " + pUpgrade.ToString() + " Range");
        transform.GetChild(2).GetComponent<TMP_Text>().SetText("Cost: $" + pCost.ToString());
    }
    
    public void PurchaseUpgrade()
    {
        if (rangeUpgrade)
        {
            for (int i = 0; i < _towerScriptableObjects.rangeUpgrades.Length; i++)
            {
                if (_towerShootScript.GetRange < _towerScriptableObjects.rangeUpgrades[i].rangeUpgrade && !_upgraded)
                {
                    _towerShootScript.ImproveRange(_towerScriptableObjects.rangeUpgrades[i].rangeUpgrade);
                    _currentCost = _towerScriptableObjects.rangeUpgrades[i + 1].cost;
                    GameManager.instance.ChargePlayer(_towerScriptableObjects.rangeUpgrades[i].cost);
                    _upgraded = true;
                }
            }

            if (_towerScriptableObjects.rangeUpgrades[_towerScriptableObjects.rangeUpgrades.Length - 1].rangeUpgrade <=
                _towerShootScript.GetRange)
            {
                gameObject.GetComponent<Button>().interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < _towerScriptableObjects.fireRateUpgrades.Length; i++)
            {
                if (_towerShootScript.GetRange < _towerScriptableObjects.fireRateUpgrades[i].fireRateUpgrade && !_upgraded)
                {
                    _towerShootScript.ImproveFireRate(_towerScriptableObjects.fireRateUpgrades[i].fireRateUpgrade);
                    _currentCost = _towerScriptableObjects.fireRateUpgrades[i + 1].cost;
                    GameManager.instance.ChargePlayer(_towerScriptableObjects.rangeUpgrades[i].cost);
                    _upgraded = true;
                }
            }

            if (_towerScriptableObjects.fireRateUpgrades[_towerScriptableObjects.fireRateUpgrades.Length - 1].fireRateUpgrade <=
                _towerShootScript.GetFireRate)
            {
                gameObject.GetComponent<Button>().interactable = false;
            }
        }
    }
}
