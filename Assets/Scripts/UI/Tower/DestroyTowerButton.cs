using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyTowerButton : MonoBehaviour
{
    [Header("Destroy Button Settings")]
    [SerializeField, Tooltip("Set the SetupShop from the scene " +
                             "so that the ui will change back once the tower has been destroyed.")]
    private SetupShop setupShop;
    
    private Button _button;
    private GameObject _target;
    
    protected void OnEnable()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonPressed);
        GameManager.onGameStateChanged += OnStateChanged;
        
        if (GameManager.instance != null)
        {
            OnStateChanged(GameManager.instance.State);
        }
    }

    protected void OnDisable()
    {
        _button.onClick.RemoveListener(OnButtonPressed);
        GameManager.onGameStateChanged -= OnStateChanged;
    }

    protected void OnDestroy()
    {
        _button.onClick.RemoveListener(OnButtonPressed);
        GameManager.onGameStateChanged -= OnStateChanged;
    }

    protected virtual void OnStateChanged(GameState state)
    {
        _button.interactable = state == GameState.building;
    }

    private void OnButtonPressed()
    {
        Destroy(_target);
        if (setupShop != null) setupShop.ChangeToBuyTowerView();
    }

    public void SetTarget(GameObject pTarget)
    {
        _target = pTarget;
    }
}
