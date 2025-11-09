using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] 
    private InputActionReference onMouseClick;

    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private TowerScriptableObjects towerScriptableObject;
    
    [SerializeField]
    private bool disappearOnClick = true;
    
    [SerializeField]
    private LayerMask excludeLayerChecks = 1<<6;

    private GridMouseTracker _gMT;
    
    private MeshRenderer _towerRenderer;
    
    private RangeRenderer _rangeRenderer;

    private bool _collided = false;

    private void Awake()
    {
        _gMT = GetComponent<GridMouseTracker>();
        _towerRenderer = GetComponent<MeshRenderer>();
        _rangeRenderer = GetComponent<RangeRenderer>();
    }

    private void OnEnable()
    {
        if (onMouseClick == null)
            return;
        
        onMouseClick.action.Enable();
        onMouseClick.action.performed += PlaceTower;
    }

    private void OnDisable()
    {
        if (onMouseClick == null)
            return;
        
        onMouseClick.action.performed -= PlaceTower;
        // onMouseClick.action.Disable();
    }

    private void Update()
    {
        if (CheckAppropriatePlacement())
        {
            _towerRenderer.material.color = Color.white;
        }
        else
        {
            _towerRenderer.material.color = Color.red;
        }
    }

    private void PlaceTower(InputAction.CallbackContext ctx)
    {
        if (towerPrefab == null)
            return;

        if (!CheckAppropriatePlacement()) return;
        
        GameObject towerObj = Instantiate(towerPrefab, transform.position, transform.rotation);
        
        if (towerObj.TryGetComponent(out SelectionScript selectionScript))
            selectionScript.SetScriptableObject(towerScriptableObject);
        
        gameObject.SetActive(!disappearOnClick);

        if (GameManager.instance != null)
            GameManager.instance.ChargePlayer(towerScriptableObject.cost);
    }

    private bool CheckAppropriatePlacement()
    {
        if (_gMT == null || _gMT.gridCellObject == null) return false;
        
        return _gMT.gridCellObject.CompareTag("TowerGround") && !_collided;
    }

    public void SetTower(GameObject pTower, TowerScriptableObjects pTowerScriptableObject)
    {
        towerPrefab = pTower;
        GetComponent<MeshFilter>().mesh = towerPrefab.GetComponent<MeshFilter>().sharedMesh;
        towerScriptableObject = pTowerScriptableObject;
        
        if (_rangeRenderer != null && towerPrefab != null && 
            towerPrefab.TryGetComponent(out TowerAttack attackScript))
        {
            _rangeRenderer.UpdateCircle(false, attackScript.CurrentAttackRange);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsIgnored(other.gameObject.layer)) return;

        _collided = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsIgnored(other.gameObject.layer)) return;

        _collided = false;
    }

    bool IsIgnored(int otherLayer)
    {
        return (excludeLayerChecks & (1 << otherLayer)) != 0;
    }
}
