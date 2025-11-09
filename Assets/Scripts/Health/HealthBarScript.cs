using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    private Slider _slider;
    private Coroutine _coroutine;

    [Header("Health Bar Settings")] 
    [SerializeField]
    private Health health;
    [SerializeField, Tooltip("Set the speed on how fast the Health Bar is changing in seconds"), Min(0.1f)]
    private float HealthBarSliderSpeed = 1;
    
    private void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = health.MaxHp;
        
        if (health != null)
        {
            health.OnHPChanged += ChangeValue;
            _slider.value = health.CurrentHp;
        }
    }

    private void OnDestroy()
    {
        if (health != null)
            health.OnHPChanged -= ChangeValue;
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
        float originalSliderValue = _slider.value;
        bool sliderValueWasLarger = _slider.value > pHp;
        while (Mathf.Abs(_slider.value - pHp) > 0.01)
        {
            _slider.value -= (originalSliderValue - pHp) * Time.deltaTime * HealthBarSliderSpeed;
            if (sliderValueWasLarger) _slider.value = Mathf.Max(pHp, _slider.value);
            else _slider.value = Mathf.Min(pHp, _slider.value);
            yield return null;
        }
    }
}