using UnityEngine;

public class BillBoardScript : MonoBehaviour
{
    [Header("Billboard Settings")]
    [SerializeField, Tooltip("Set to true if the GameObject needs to stay above the GameObject it's attached to.")]
    private bool topOfObject = false;
        
    void Update()
    {
        if (topOfObject)
        {
            transform.rotation = Quaternion.Euler(90, -transform.parent.rotation.y, -transform.parent.rotation.z);
            transform.position = transform.parent.position + new Vector3(0,0,1);
        }
        else
        {
            Vector3 directionToCamera = Camera.main.transform.position - transform.position;

            transform.rotation = Quaternion.LookRotation(-directionToCamera);
        }
    }
}
