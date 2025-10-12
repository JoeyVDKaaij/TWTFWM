using System;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    private void OnEnable()
    {
        if (WaveManager.instance != null)
            WaveManager.instance.AddSpawner(this);
    }

    private void OnDisable()
    {
        if (WaveManager.instance != null)
            WaveManager.instance.RemoveSpawner(this);
    }

    public void SpawnGameObject(GameObject pObj)
    {
        if (pObj == null) return;
        
        Debug.Log("Instantiating Object");
        Instantiate(pObj, transform);
    }
}
