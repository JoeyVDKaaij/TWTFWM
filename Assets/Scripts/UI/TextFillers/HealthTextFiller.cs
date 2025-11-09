public class HealthTextFiller : TMPTextFiller
{
    protected override void Awake()
    {
        base.Awake();
        if (GameManager.instance != null && GameManager.instance.playerHp != null)
            GameManager.instance.playerHp.OnHPChanged += SetNumber;
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null && GameManager.instance.playerHp != null)
            GameManager.instance.playerHp.OnHPChanged -= SetNumber;
    }
}