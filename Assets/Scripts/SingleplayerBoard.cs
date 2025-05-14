using UnityEngine;

public class SingleplayerBoard : Board
{
    public override void SelectedPieceMoved(Vector2 coords)
    {
        Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x), Mathf.RoundToInt(coords.y));  
        OnSelectedPieceMove(intCoords);
    }

    public override void SetSelectedPiece(Vector2 coords)
    {
        Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x),Mathf.RoundToInt(coords.y));
        OnSetSelectedPiece(intCoords);
    }
}
