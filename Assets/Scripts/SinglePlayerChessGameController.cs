using UnityEngine;

public class SinglePlayerChessGameController : ChessGameController
{
    public override bool CanPerformMove()
    {
        if(!IsGameInProgress())
            return false;
        return true;
    }

    public override void TryToStartCurrentGame()
    {
        SetGameState(GameState.Play);
    }

    protected override void SetGameState(GameState state)
    {
        this.gameState = state;
    }
}
