public class PlayerHealth : Health 
{
    protected override void Death()
    {
        if (GameManager.instance != null)
            GameManager.instance.ChangeGameState(GameState.gameOver);
    }
}