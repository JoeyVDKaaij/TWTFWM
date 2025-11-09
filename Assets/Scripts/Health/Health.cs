using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action<float> OnHPChanged;
    
    private float _currentHp;
    public float CurrentHp
    {
        get { return _currentHp; }
        private set
        {
            _currentHp = value;
            OnHPChanged?.Invoke(_currentHp);
            
            if (_currentHp <= 0) Death();
            
            #if UNITY_EDITOR
            if (enableDebuggingMode) Debug.Log(_currentHp);
            #endif
        }
    }
    
    [Header("Health Settings")]
    [SerializeField, Tooltip("Set max HP"), Min(1)]
    private float maxHp;
    public float MaxHp { get => maxHp; private set => maxHp = value; }

    [SerializeField, Tooltip("Set HP that the object starts off with."), Min(1)]
    private float startingHp = 100;
    
    [SerializeField, Tooltip("Enables debugging shortcuts that allows for testing the health features." +
                             "Press I for -10 HP, O for +10 HP and P to set the HP back to max HP.")]
    protected bool enableDebuggingMode = false;

    // Ensuring starting HP never goes beyond maxHp
    private void OnValidate()
    {
        if (maxHp < startingHp) startingHp = maxHp;
    }

    protected virtual void Awake()
    {
        _currentHp = startingHp;
    }

    public void TakeDamage(float pDamage)
    {
        CurrentHp -= pDamage;
    }

    public void Heal(float pHeal)
    {
        if (CurrentHp + pHeal >= maxHp) 
            CurrentHp = maxHp;
        else 
            CurrentHp += pHeal;
    }

    protected void SetHealth(float pHealth)
    {
        CurrentHp = pHealth;
    }

    protected void SetMaxHealth(float pMaxHealth, bool pUpdateCurrentHp = false)
    {
        MaxHp = pMaxHealth;
        if (pUpdateCurrentHp) CurrentHp += pMaxHealth - CurrentHp;
    }

    protected virtual void Death() { Debug.Log("Death"); }
    
    #region DEBUGGING
#if UNITY_EDITOR
    protected void Update()
    {
        if (!enableDebuggingMode) return;
        if (Input.GetKeyDown(KeyCode.I)) TakeDamage(10);
        if (Input.GetKeyDown(KeyCode.O)) Heal(10);
        if (Input.GetKeyDown(KeyCode.P)) SetHealth(MaxHp);
    }
#endif
    #endregion
    
    
    public void InvokeOnHealthChanged()
    {
        OnHPChanged?.Invoke(_currentHp);
    }
}