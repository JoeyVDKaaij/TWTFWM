using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager instance { get; private set; }

    private GameState state = GameState.building;
    
    // [Header("Tower Settings")] 
    
    private GameObject previewTower = null;
    private Vector3 mousePos;
    private Vector3 lockedPos = new Vector3(0,0,0);
    private TowerScriptableObjects tower = null;
    private bool hitWall = false;
    private bool hitTower = false;
    
    private Renderer previewTowerRenderer = null;
    
    #region UnityFunctions
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            if (transform.parent.gameObject != null) DontDestroyOnLoad(transform.parent.gameObject);
            else DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameManager.onGameStateChanged += ChangeGameState;
    }
    
    void Start()
    {
        if (previewTower != null)
        { 
            previewTower.transform.position = lockedPos;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
        
        GameManager.onGameStateChanged -= ChangeGameState;
    }
    
    void Update()
    {
        if (previewTower != null) TowerPlacement();
        
        if (Input.GetMouseButtonDown(2)) DisableTowerPlacement();
    }
    
    #endregion

    #region TowerPlacement

    private void TowerPlacement()
    {
        mousePos = Input.mousePosition;

        mousePos.z = 18.24f;

        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit[] hits = Physics.RaycastAll(mousePos - transform.up, transform.up);
        GameObject spawnedTower = null;
        SetLockedPos();   
        previewTower.transform.position = lockedPos;
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.CompareTag("Wall"))
                {
                    SetLockedPos(hits[i].collider.gameObject.transform);
                    previewTower.transform.position = lockedPos;
                    hitWall = true;
                }
                else if (hits[i].collider.CompareTag("Tower"))
                {
                    spawnedTower = hits[i].collider.gameObject;
                    if (spawnedTower != previewTower) hitTower = true;
                }
                
            }
        }

        if (hitWall)
        {
            if (Input.GetMouseButtonDown(0) && !hitTower)
            {
                GameObject spawningTower =
                    Instantiate(previewTower, previewTower.transform.position, transform.rotation);
                spawningTower.AddComponent<ShootScript>();
                spawningTower.GetComponent<ShootScript>().SetValues(tower);
                GameManager.instance.ChargePlayer(tower.price);
                DisableTowerPlacement();
                hitWall = false;
            }
        }

        if (hitTower && previewTowerRenderer != null || !hitWall && previewTowerRenderer != null)
        {
            // Call SetColor using the shader property name "_Color" and setting the color to red
            previewTowerRenderer.material.color = Color.red;
        }
        else
        {
            previewTowerRenderer.material.color = Color.grey;
        }

        hitWall = false;
        hitTower = false;
    }

    private void SetLockedPos(Transform pWallTransform = null)
    {
        if (pWallTransform != null)
        {
            lockedPos = pWallTransform.position;
            lockedPos.y += 1;
        }
        
        float xWallPosition = Mathf.Round(mousePos.x - lockedPos.x);
        lockedPos.x += xWallPosition;
        
        float zWallPosition = Mathf.Round(mousePos.z - lockedPos.z);
        lockedPos.z += zWallPosition;
    }

    #endregion
    
    private void ChangeGameState(GameState pState)
    {
        state = pState;
    }

    public void EnableTowerPlacement(TowerScriptableObjects pTower)
    {
        if (previewTower != null)
        {
            DestroyImmediate(previewTower);
            previewTowerRenderer = null;
        }
        tower = pTower;
        if (pTower.tower != null)
        {
            previewTower = Instantiate(tower.tower, new Vector3(0,0,0), Quaternion.Euler(0,0,0));
            previewTowerRenderer = previewTower.GetComponent<Renderer>();
        }
        else Debug.LogError(pTower.name + " has no prefab assigned!");
    }

    public void DisableTowerPlacement()
    {
        previewTowerRenderer = null;
        Destroy(previewTower);
        tower = null;
    }

    #region Getters

    public TowerManager Instance
    {
        get { return instance; }
    }
    
    #endregion
}