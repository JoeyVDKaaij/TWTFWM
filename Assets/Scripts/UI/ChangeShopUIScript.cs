using UnityEngine;

public class ChangeShopUIScript : MonoBehaviour
{
    [SerializeField, Tooltip("The shop UI when no tower is selected")]
    private GameObject towerShopUI = null;
    [SerializeField, Tooltip("The UI that shows when the tower is selected")]
    private GameObject selectedTowerUI = null;

    private bool _selectedTower;
    private bool _mouseHoveringOverUI;
    private bool _delayEventInvoke;
    
    // Start is called before the first frame update
    void Start()
    {
        GuiManager.ChangeTowerUI += ChangeShop;
    }

    private void OnDestroy()
    {
        GuiManager.ChangeTowerUI -= ChangeShop;
    }

    private void ChangeShop(bool pTowerSelected, GameObject pTower = null)
    {
        towerShopUI.SetActive(!pTowerSelected);
        selectedTowerUI.SetActive(pTowerSelected);

        _selectedTower = pTowerSelected;
        _delayEventInvoke = true;
    }

    public void MouseEnterUI()
    {
        _mouseHoveringOverUI = true;
    }

    public void MouseExitUI()
    {
        _mouseHoveringOverUI = false;
    }
    
    private void Update()
    {
        if (!_mouseHoveringOverUI && _selectedTower && Input.GetMouseButtonDown(0) && !_delayEventInvoke)
            GuiManager.instance.ChangeTowerMenu(false);
                
        _delayEventInvoke = false;
    }
}
