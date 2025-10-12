using UnityEditor;
using UnityEngine;

public class SelectTowerScript : MonoBehaviour
{
    public TowerScriptableObjects towerScriptableObjects;
    
    private bool towerSelected = false;
    private float shootRange = 0;

    private void Start()
    {
        GuiManager.ChangeTowerUI += deselectedTower;
    }

    private void OnDestroy()
    {
        GuiManager.ChangeTowerUI -= deselectedTower;
    }

    private void OnGUI()
    {
        if (towerSelected)
        {
#if UNITY_EDITOR
            Handles.DrawWireDisc(transform.position, transform.up, shootRange);
#endif
        }
    }
    
    private void OnMouseDown()
    {
        towerSelected = true;
        if (GuiManager.instance != null)
            GuiManager.instance.ChangeTowerMenu(true, gameObject);
    }
    
    private void deselectedTower(bool pSelectedATower, GameObject pTower)
    {
        towerSelected = pTower == gameObject;
    }
}
