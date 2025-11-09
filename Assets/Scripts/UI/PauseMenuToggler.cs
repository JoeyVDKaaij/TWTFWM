using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PauseMenuToggler : MonoBehaviour
{
    [FormerlySerializedAs("clickAction")] [SerializeField, Tooltip("Set the Mouse Click Input Action.")]
    private InputActionReference pauseMenuAction;
    [SerializeField, Tooltip("Set the game object holding the pause menu.")]
    private GameObject pauseMenu;

    private void Awake()
    {
        pauseMenuAction.action.Enable();
        pauseMenuAction.action.performed += OnPauseMenuToggle;
    }

    private void OnDestroy()
    {
        pauseMenuAction.action.Disable();
        pauseMenuAction.action.performed -= OnPauseMenuToggle;
    }

    private void OnPauseMenuToggle(InputAction.CallbackContext ctx)
    {
        if (pauseMenu == null) return;
        
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        
        if (GameManager.instance != null)
        {
            GameManager.instance.ChangeGameSpeed(pauseMenu.activeSelf ? 0 : 1);
        }
    }
}
