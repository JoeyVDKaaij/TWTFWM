public class BuildingPhaseCountdownTextFiller : TMPTextFiller
{
    private bool buildState;
    protected override void Awake()
    {
        base.Awake();
        GameManager.onGameStateChanged += CheckState;
    }

    private void Update()
    {
        if (!buildState)
        {
            SetTextEmpty();
            return; 
        }

        if (WaveManager.instance != null)
        {
            SetNumber(WaveManager.instance.GetBuildingPhaseTimer);
        }
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= CheckState;
    }
    
    private void CheckState(GameState state)
    {
        buildState = state == GameState.building;
    }
}