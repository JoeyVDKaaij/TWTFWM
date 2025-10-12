using UnityEngine;
using UnityEngine.AI;

public class PathFindingMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField, Tooltip("Set the speed the GameObject is moving in units per second."), Min(0)]
    private float movementSpeed = 1.0f;
    private bool moving = true;
    
    // agent will be set in the Start function.
    //The user only has to apply the NavMeshAgent component to the GameObject this string is attached to.
    [Header("Pathfinding Settings")]
    [SerializeField, Tooltip("Set the target that the ai should move this GameObject to (Not mandatory).")]
    private Transform target = null;
    private NavMeshAgent agent = null;

    [SerializeField, Tooltip("Turn on debugging mode to test out movement features inside the editor.")]
    private bool debuggingMode = false;
    
    private float debufTimer = 0;
        
    #region Unity Functions
    
    private void Awake()
    {
        // Initialize the agent variable and the target variable if it's not set already.
        agent = GetComponent<NavMeshAgent>();
        if (target == null)
            target = GameObject.FindGameObjectWithTag("EndPoint")?.transform;
        
        GameManager.onGameStateChanged += ToggleMovementsOnGameState;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= ToggleMovementsOnGameState;
    }
    
    void Update()
    {
        if (AbleToMove())
        {
            PathFinding();
        }

        if (Input.GetKeyDown(KeyCode.Space) && debuggingMode) ToggleMovements();
    }

    #endregion

    #region Movements

    private void PathFinding()
    {
        if (target != null)
        {
            if (agent != null)
            {
                agent.SetDestination(target.position);
                agent.speed = movementSpeed;
            }
            else Debug.LogError("Agent component is not active.");
        }
        else Debug.LogError("Cannot find GameObject for target.");
    }
    
    private void ToggleMovements()
    {
        moving = !moving;
    }

    private void ToggleMovementsOnGameState(GameState state)
    {
        // Move the GameObject if the game is not paused or if the game isn't over.
        moving = state == GameState.play;
    }

    public void SlowDownSpeed(float pDecreasedSpeed, float pDebufTimer, bool pAbsoluteDecreasedSpeed = false)
    {
        if (debufTimer > 0)
        {
            if (pAbsoluteDecreasedSpeed)
                movementSpeed = 0 - pDecreasedSpeed;
            else
                movementSpeed -= pDecreasedSpeed;
        }

        debufTimer = pDebufTimer;
    }

    private bool AbleToMove()
    {
        if (GameManager.instance != null)
        {
            return GameManager.instance.state == GameState.play;
        }
        
        return moving;
    }
    
    #endregion
}