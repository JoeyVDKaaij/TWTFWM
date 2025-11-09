using UnityEngine;

/// <summary>
/// Base class for movement.
/// Subclasses are created from this base class for all different types of movement.
/// </summary>
public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField, Tooltip("Set the speed the GameObject is moving in units per second."), Min(0)]
    protected float movementSpeed = 1.0f;
    [SerializeField, NonEditable, Tooltip("Shows if the game object is allowed to move.")]
    protected bool moving = true;

    [Header("Debugging")]
    [SerializeField, Tooltip("Turn on debugging mode to test out movement features inside the editor." +
                             " Press space to toggle movement.")]
    private bool debuggingMode = false;

    protected float currentSpeed;

    protected bool movementDebuff = false;
    protected Timer movementDebuffTimer;

    private void OnValidate()
    {
        currentSpeed = movementSpeed;
    }

    protected virtual void Awake()
    {
        GameManager.onGameStateChanged += ToggleMovementsOnGameState;
        currentSpeed = movementSpeed;
    }

    protected void Start()
    {
        if (GameManager.instance != null)
            ToggleMovementsOnGameState(GameManager.instance.State);
    }

    protected void OnDestroy()
    {
        GameManager.onGameStateChanged -= ToggleMovementsOnGameState;
    }

    protected virtual void Update()
    {
        if (movementDebuff)
        {
            movementDebuffTimer.UpdateTimer();
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && debuggingMode) ToggleMovements();
    }
    
    #region Movements
    
    protected void ToggleMovements()
    {
        moving = !moving;
    }

    protected void ToggleMovementsOnGameState(GameState state)
    {
        // Move the GameObject if the game is not paused or if the game isn't over.
        moving = state == GameState.play;
    }

    public void SpeedDebuff(float pNewSpeed, float pDebuffTime)
    {
        currentSpeed = pNewSpeed;
        movementDebuff = true;

        if (movementDebuffTimer == null)
        {
            movementDebuffTimer = new Timer(pDebuffTime, ResetSpeed);
        }
        else
        {
            movementDebuffTimer.UpdateDeadline(pDebuffTime);
            movementDebuffTimer.ResetTimer();
        }
    }

    public void ResetSpeed()
    {
        currentSpeed = movementSpeed;
        movementDebuff = false;
    }
    
    #endregion
}