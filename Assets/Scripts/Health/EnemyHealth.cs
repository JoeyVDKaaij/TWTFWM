using UnityEngine;
using UnityEngine.Serialization;

public class EnemyHealth : Health
{
    [Header("Enemy Settings")]
    [SerializeField, Tooltip("The amount of money the enemy possess"), Min(0)]
    private int money = 1;
    [SerializeField, Tooltip("Set the prefab showing how much money you gained.")]
    private GameObject moneyGainedPrefab = null;
    
    protected override void Awake()
    {
        base.Awake();
        if (money < 0) money = 0;
    }

    protected override void Death()
    {
        if (GameManager.instance != null)
            GameManager.instance.AddMoney(money);
        if (WaveManager.instance != null)
            WaveManager.instance.EnemyDied();
        
        ShowMoneyGained();
        
        Destroy(gameObject);
    }

    private void ShowMoneyGained()
    {
        if (moneyGainedPrefab == null) return;
        
        GameObject moneyGainedObject = Instantiate(moneyGainedPrefab, transform.position, moneyGainedPrefab.transform.rotation);

        if (moneyGainedObject.TryGetComponent(out TextFillerGetter tFP))
        {
            MoneyGainedTextFiller moneyGainedTextFiller = (MoneyGainedTextFiller)tFP.TextFiller;
            moneyGainedTextFiller.SetMoneyAmount(money);
        }
    }
}