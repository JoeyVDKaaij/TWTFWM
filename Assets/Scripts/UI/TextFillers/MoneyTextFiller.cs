public class MoneyTextFiller : TMPTextFiller
{
    protected override void Awake()
    {
        base.Awake();
        GameManager.onMoneyChanged += SetNumber;
    }

    private void OnDestroy()
    {
        GameManager.onMoneyChanged -= SetNumber;
    }
}