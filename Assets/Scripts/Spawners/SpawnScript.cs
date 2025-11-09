using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public virtual GameObject SpawnGameObject(GameObject pObj)
    {
        if (pObj == null) return null;
        
        Debug.Log("Instantiating Object");
        return Instantiate(pObj, transform);
    }
}
