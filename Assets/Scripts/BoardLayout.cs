using System;
using UnityEngine;

[CreateAssetMenu(menuName =" Scriptable Objects/Board/Layout")]
public class BoardLayout : ScriptableObject
{
    [Serializable]
    private class BoardSquareSetUp
    {
        public Vector2Int position;
        public PieceType pieceType;
        public TeamColor teamColor;
    }
    [SerializeField] private BoardSquareSetUp[] boardSquares;

    public int GetPiecesCount()
    {
        return boardSquares.Length;
    }
    public Vector2Int GetSquareCoordsAtIndex(int index)
    {
        if(boardSquares.Length <= index)
        {
            Debug.LogError("Index of piece is out of range");
            return new Vector2Int(-1, -1);
        }
        return new Vector2Int(boardSquares[index].position.x-1, boardSquares[index].position.y-1);
    }
    public string GetSquarePieceNameAtIndex(int index)
    {
        if (boardSquares.Length <= index)
        {
            Debug.LogError("Index of piece is out of range");
            return "";
        }
        return boardSquares[index].pieceType.ToString();
    }
    public TeamColor GetTeamColorAtIndex(int index)
    {
        if (boardSquares.Length <= index)
        {
            Debug.LogError("Index of piece is out of range");

            return TeamColor.Black;
        }
        return boardSquares[index].teamColor;
    }
}
