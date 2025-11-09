using UnityEngine;
using UnityEngine.AI;

public class PathFindingMovement : Movement
{
    // agent will be set in the Start function.
    //The user only has to apply the NavMeshAgent component to the GameObject this string is attached to.
    [Header("Pathfinding Settings")]
    [SerializeField, Tooltip("Set the target that the ai should move this GameObject to (Not mandatory).")]
    private Transform target = null;
    private NavMeshAgent agent = null;
        
    #region Unity Functions

    protected override void Awake()
    {
        base.Awake();
        
        // Initialize the agent variable
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            Debug.LogError("Agent component is not active.");
    }

    protected override void Update()
    {
        base.Update();
        
        if (moving)
        {
            PathFinding();
        }
    }

    #endregion

    private void PathFinding()
    {
        if (target == null || agent == null) return;
        
        agent.SetDestination(target.position);
        agent.speed = currentSpeed;
    }

    public void SetTarget(Transform pTarget)
    {
        target = pTarget;
    }
}