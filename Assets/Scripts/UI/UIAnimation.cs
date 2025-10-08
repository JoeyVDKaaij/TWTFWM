using UnityEngine;
using UnityEngine.EventSystems;

public class UIAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// Set the animator of the GUI and slide the tower menu to the left when hovering over it.
    /// </summary>
    [Header("Animator settings")]
    [SerializeField, Tooltip("The animator that animates the towerui sliding to the left")]
    private Animator animator = null;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Enable animator upon first activation.
        animator.enabled = true;
        animator.SetBool("TowerUIActive", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Enable animator upon first activation.
        animator.enabled = true;
        animator.SetBool("TowerUIActive", false);
    }
}