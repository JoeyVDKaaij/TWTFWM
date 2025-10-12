using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HpBarScript : MonoBehaviour
{
    private Slider _slider;
    private Coroutine _coroutine;

    [FormerlySerializedAs("_enemyHp")]
    [Header("HP Bar Settings")] 
    [SerializeField]
    private Health _hp;
    [SerializeField, Tooltip("Set the speed on how fast the HP Bar is changing"), Min(0.1f)]
    private float HpBarSliderSpeed = 1;
    
    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = _hp.MaxHp;
        
        if (_hp != null)
        {
            _hp.OnHPChanged += ChangeValue;
            _slider.value = _hp.CurrentHp;
        }
    }

    private void OnDestroy()
    {
        if (_hp != null)
            _hp.OnHPChanged -= ChangeValue;
    }

    private void ChangeValue(float pHp)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(ChangeSliderValue(pHp));
    }

    /// <summary>
    /// Change the slider by scrolling it down smoothly instead of setting it directly.
    /// </summary>
    /// <param name="pHp">The current HP that the player is on.</param>
    IEnumerator ChangeSliderValue(float pHp)
    {
        while (Mathf.Abs(_slider.value - pHp) > 0.01)
        {
            _slider.value -= (_slider.value - pHp) * Time.deltaTime * HpBarSliderSpeed;
            yield return null;
        }
    }
}