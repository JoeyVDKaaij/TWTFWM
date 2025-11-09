using UnityEngine;

public class BillBoardScript : MonoBehaviour
{
    [Header("Billboard Settings")]
    [SerializeField, Tooltip("Set to true if the GameObject needs to rotate towards the upwards direction. " +
                             "Otherwise look directly at the camera. ")]
    private bool lookUp = false;
        
    void Update()
    {
        if (lookUp)
        {
            transform.rotation = Quaternion.Euler(90, -transform.parent.rotation.y, -transform.parent.rotation.z);
        }
        else
        {
            Vector3 directionToCamera = Camera.main.transform.position - transform.position;

            transform.rotation = Quaternion.LookRotation(-directionToCamera);
        }
    }
}
