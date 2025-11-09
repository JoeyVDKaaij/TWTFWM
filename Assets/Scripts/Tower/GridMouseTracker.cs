using UnityEngine;
using UnityEngine.InputSystem;

public class GridMouseTracker : MonoBehaviour
{
    [SerializeField]
    InputActionReference inputAction;
    
    [SerializeField]
    private LayerMask groundMask = 1<<6;
    
    private Vector3 _worldPos;
    private Camera _mainCam;

    public GameObject gridCellObject { get; private set; } = null;

    private void OnEnable()
    {
        inputAction.action.Enable();

        // Moves the cube off screen so that there is no visual bugs visible.
        transform.position = transform.position + Vector3.right * 10;
    }

    private void OnDisable()
    {
        inputAction.action.Disable();
    }

    private void Start()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        Ray ray = _mainCam.ScreenPointToRay(inputAction.action.ReadValue<Vector2>());
        
        ray.origin += Vector3.up;
        
        if (Physics.Raycast(ray, out RaycastHit hit, 1000, groundMask))
        {
            gridCellObject = hit.collider.gameObject;

            MoveObject(hit.collider);
        }
        else
        {
            gridCellObject = null;
        }
    }
    
    private void MoveObject(Collider pCollider)
    {
        Vector3 pos = pCollider.transform.position;
        
        if (pCollider.TryGetComponent(out Renderer rend))
            pos.y += rend.bounds.size.y;

        transform.position = pos;
    }
}
