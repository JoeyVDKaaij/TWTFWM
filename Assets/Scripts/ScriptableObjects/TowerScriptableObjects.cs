using UnityEngine;

[CreateAssetMenu(fileName = "Tower", menuName = "TowerDefenseAssets/Tower", order = 1)]
public class TowerScriptableObjects : ScriptableObject
{
    public GameObject towerPrefab;
    [Min(0)]
    public int cost;
    public Sprite icon;
}