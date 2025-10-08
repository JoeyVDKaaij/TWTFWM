using UnityEngine;
using UnityEngine.UI;

public class HpBarScript : MonoBehaviour
{
    private EnemyScript enemyScript;
    private Slider _slider;
    private float currentHp;

    [Header("HP Bar Settings")] 
    [SerializeField, Tooltip("Set the speed on how fast the HP Bar is changing"), Min(0.1f)]
    private float HpBarSliderSpeed = 1;
    
    void Awake()
    {
        enemyScript = transform.parent.parent.GetComponent<EnemyScript>();
        enemyScript.OnHPChanged += ChangeValue;

        _slider = GetComponent<Slider>();
        _slider.maxValue = enemyScript.GetMaxHealth;
        currentHp = _slider.maxValue;
        _slider.value = _slider.maxValue;
    }

    private void OnDestroy()
    {
        enemyScript.OnHPChanged -= ChangeValue;
    }
    
    void Update()
    {
        if (_slider.value != currentHp)
        {
            _slider.value -= (_slider.value - currentHp) * Time.deltaTime * HpBarSliderSpeed;
        }
    }

    private void ChangeValue(float pDamage)
    {
        currentHp -= pDamage;
    }
}