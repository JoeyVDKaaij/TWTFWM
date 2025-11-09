using System;
using UnityEngine;

public class SelectionScript : MonoBehaviour
{
    public static event Action<UpgradeScript, Sprite, LineRenderer> OnSelection;
    
    private UpgradeScript _upgrade;
    
    private TowerScriptableObjects _tower;
    
    private LineRenderer _lineRend;

    private void Awake()
    {
        _upgrade = GetComponent<UpgradeScript>();
        _lineRend = GetComponent<LineRenderer>();
        
        if (_lineRend != null) _lineRend.enabled = false;
    }

    private void OnMouseDown()
    {
        OnSelection?.Invoke(_upgrade, _tower.icon, _lineRend);
        
        if (_lineRend != null) _lineRend.enabled = true;
    }

    public void SetScriptableObject(TowerScriptableObjects pTower)
    {
        _tower = pTower;
    }
}