using UnityEngine;

public class WaveTextFiller : TMPTextFiller
{
    private void Start()
    {
        if (WaveManager.instance != null)
        {
            WaveManager.instance.onWaveChanged += SetNumber;
        }
        else 
            Debug.LogWarning("WaveManager is null. Wave Text won't be filled.");
    }

    private void OnDestroy()
    {
        if (WaveManager.instance != null)
        {
            WaveManager.instance.onWaveChanged -= SetNumber;
        }
    }
}