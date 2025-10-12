using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class MovementScript : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField, Tooltip("Set the speed the GameObject is moving in units per second."), Min(0)]
    private float movementSpeed = 1.0f;
    [SerializeField, Tooltip("Set to true if the GameObject has to move in a certain path.")]
    private MovementType movementType = MovementType.oneAxisMovement;
    private bool moving = true;

    private float originalSpeed;

    [Header("Axis Settings")]
    [SerializeField, Tooltip("Set which axis the GameObject has to move.")]
    private Axis axis = Axis.x;
    [SerializeField, Tooltip("Set to true to make the GameObject move in reverse.")]
    private bool reverseMovement = false;
    [SerializeField, Tooltip("Set to false to make the GameObject move where it is rotated towards.")]
    private bool moveInWorldSpace = false;
    

    [Header("Pathfinding Settings")]
    // agent will be set in the Start function.
    //The user only has to apply the NavMeshAgent component to the GameObject this string is attached to.
    [SerializeField, Tooltip("Set the target that the ai should move this GameObject to (Not mandatory).")]
    private Transform target = null;
    private NavMeshAgent agent = null;


    [Header("Static Paths Settings")]
    [SerializeField, Tooltip("Set the points of the path from objectspace.")]
    private Vector3[] pathPoints = null;
    [SerializeField, Tooltip("Set to true if the endpoint and starting point should connect. Creating a loop.")]
    private bool loopPath;
    [SerializeField, Tooltip("Set to true if the GameObject shoud destroy itself when it reaches the final pathPoint (cannot be enabled if loopPath is true).")]
    private bool destroyOnLastPoint;
    private int pathPointID = 0;
    private float startTime, journeyLength;

    private float debufTimer = 0;
    
    #region Unity Functions
    
    private void Awake()
    {
        // Initialize the agent variable and the target variable if it's not set already.
        agent = GetComponent<NavMeshAgent>();
        if (target == null)
            target = GameObject.FindGameObjectWithTag("EndPoint").transform;

        if (movementType == MovementType.staticPath)
        {
            // Setting the pathpoints to it's local transform.
            for (int i = 0; i < pathPoints.Length; i++) pathPoints[i] += transform.position;

            startTime = Time.time;
            
            journeyLength = Vector3.Distance(pathPoints[pathPointID], pathPoints[pathPointID+1]);
        }

        originalSpeed = movementSpeed;
        
        GameManager.onGameStateChanged += ToggleMovementsOnGameState;
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= ToggleMovementsOnGameState;
    }
    
    void Update()
    {
        if (moving)
        {
            switch (movementType)
            {
                case MovementType.oneAxisMovement:
                    OneAxisMovement();
                    break;

                case MovementType.pathFinding:
                    PathFinding();
                    break;

                case MovementType.staticPath:
                    StaticPath();
                    break;

                default:
                    Debug.LogError("movementType not set.");
                    break;
            }
        }

        if (debufTimer <= 0)
            movementSpeed = originalSpeed;
        else debufTimer -= Time.deltaTime;
    }

    #endregion
    
    #region Gizmos

    private void OnDrawGizmos()
    {
        if (movementType == MovementType.staticPath && pathPoints.Length > 0)
        {
            for (int i = 0; i < pathPoints.Length; i++)
            {
                Color previousContentColor = GUI.contentColor;
                GUI.contentColor = Color.black;
                
#if UNITY_EDITOR
                Handles.color = Color.gray;

                Handles.color = Color.black;
                Handles.Label(transform.localPosition + pathPoints[i], (i).ToString());
#endif
                
                GUI.contentColor = previousContentColor;

                Gizmos.color = Color.yellow;
                if (i + 1 < pathPoints.Length && pathPoints.Length > 1)
                    Gizmos.DrawLine(transform.localPosition + pathPoints[i], transform.localPosition + pathPoints[i + 1]);
                else if (pathPoints.Length > 2 && loopPath)
                    Gizmos.DrawLine(transform.localPosition + pathPoints[pathPoints.Length - 1], transform.localPosition + pathPoints[0]);
            }
        }
    }

    #endregion

    #region Movements

    private void OneAxisMovement()
    {
        // Depending on the set direction. The GameObject moves in that axis with the set movementSpeed.
        float axisMovementSpeed = movementSpeed * Time.deltaTime;
        if (reverseMovement) axisMovementSpeed = -movementSpeed * Time.deltaTime;

        switch (axis)
        {
            case Axis.x:
                if (moveInWorldSpace) transform.position += new Vector3(axisMovementSpeed, 0, 0);
                else transform.Translate(new Vector3(axisMovementSpeed, 0, 0));
                break;
            case Axis.y:
                if (moveInWorldSpace) transform.position += new Vector3(0, axisMovementSpeed, 0);
                else transform.Translate(new Vector3(0, axisMovementSpeed, 0));
                break;
            case Axis.z:
                if (moveInWorldSpace) transform.position += new Vector3(0, 0, axisMovementSpeed);
                else transform.Translate(new Vector3(0, 0, axisMovementSpeed));
                break;
        }
    }

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

    private void StaticPath()
    {
        // StaticPath code
        if (pathPoints != null)
        {
            float distCovered = (Time.time - startTime) * movementSpeed;

            float fractionOfJourney = distCovered / journeyLength;

            fractionOfJourney = Mathf.Clamp01(fractionOfJourney);

            if (pathPointID + 1 < pathPoints.Length)
                transform.position = Vector3.Lerp(pathPoints[pathPointID], pathPoints[pathPointID + 1], fractionOfJourney);
            else
                transform.position = Vector3.Lerp(pathPoints[pathPointID], pathPoints[0], fractionOfJourney);

            if (fractionOfJourney >= 1f)
            {
                startTime = Time.time;
                if (pathPointID + 1 < pathPoints.Length)
                {
                    pathPointID++;
                    journeyLength = Vector3.Distance(pathPoints[pathPointID], pathPoints[pathPointID + 1]);
                }
                else if (loopPath)
                {
                    journeyLength = Vector3.Distance(pathPoints[pathPointID], pathPoints[0]);
                    pathPointID = 0;
                }
                else if (destroyOnLastPoint)
                {
                    Destroy(gameObject);
                }
            }
        }
        else Debug.LogError("pathPoints isn't set.");
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

    #endregion
}

public enum Axis
{
    x,
    y,
    z
}

public enum MovementType
{
    oneAxisMovement,
    pathFinding,
    staticPath
}