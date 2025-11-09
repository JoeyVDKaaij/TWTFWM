using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SetupShop : MonoBehaviour
{
    [SerializeField, Tooltip("Set all the tower that needs to appear in the shop.")]
    private TowerScriptableObjects[] towerScriptableObjects;
    [SerializeField, Tooltip("Set the prefab of the button that allows you to buy towers.")] 
    private GameObject buyButtonPrefab;
    [SerializeField, Tooltip("Set the container that stores all the buttons as child objects.")] 
    private GameObject buyButtonContainer;
    [SerializeField, Tooltip("Set the object that does the placement of the towers.")] 
    private TowerPlacement towerPlacementGuide;
    
    [SerializeField, Tooltip("Set the UI that handles when a tower is selected.")] 
    private UpgradeShopSetupScript towerSelectionUI;
    
    // Saving the list for future use.
    private List<GameObject> buyButtons = new List<GameObject>();
    
    [SerializeField, Tooltip("Set the Mouse Click Input Action.")]
    private InputActionReference clickAction;
    
    private LineRenderer selectedTowerLineRend;
    
    private void Awake()
    {
        foreach (TowerScriptableObjects tSO in towerScriptableObjects)
        {
            buyButtons.Add(Instantiate(buyButtonPrefab, buyButtonContainer.transform));
            PurchaseButton pB = buyButtons[^1].GetComponent<PurchaseButton>();
            pB.SetTower(tSO, towerPlacementGuide);
        }

        SelectionScript.OnSelection += OnTowerClicked;
        clickAction.action.Enable();
        clickAction.action.performed += OnClick;
    }

    void OnDestroy()
    {
        clickAction.action.performed -= OnClick;
        SelectionScript.OnSelection -= OnTowerClicked;
        clickAction.action.Disable();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        if (IsPointerOverPanel() || towerSelectionUI == null || buyButtonContainer == null) return;

        ChangeToBuyTowerView();
    }

    private bool IsPointerOverPanel()
    {
        if (!EventSystem.current) return false;

        // Raycast to check if it's over this panel or children
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Mouse.current.position.ReadValue()
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.transform.IsChildOf(transform))
                return true;
        }

        return false;
    }

    private void OnTowerClicked(UpgradeScript pUS, Sprite pTowerIcon, LineRenderer pLineRend)
    {
        if (towerSelectionUI == null && buyButtonContainer == null) return;
        
        towerSelectionUI.gameObject.SetActive(true);
        buyButtonContainer.SetActive(false);

        if (pUS != null)
        {
            towerSelectionUI.SetupShop(pUS, pTowerIcon);
        }

        selectedTowerLineRend = pLineRend;
    }

    public void ChangeToBuyTowerView()
    {
        towerSelectionUI.gameObject.SetActive(false);
        buyButtonContainer.SetActive(true);

        if (selectedTowerLineRend != null)
        {
            selectedTowerLineRend.enabled = false;
        }
    }
}
