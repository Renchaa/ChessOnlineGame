using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SquareSelectorCreator))]
public class Board : MonoBehaviour
{
    public const int BOARD_SIZE = 8;

    [SerializeField] private Transform bottomLeftSquareTransform;
    [SerializeField] private float squareSize;

    private Piece[,] grid;
    private Piece selectedPiece;
    private ChessGameController gameController;
    private SquareSelectorCreator squareSelectorCreator;
    private void Awake()
    {
        squareSelectorCreator = GetComponent<SquareSelectorCreator>();
        CreaterGrid();
    }
    public void SetDependencies(ChessGameController gameController)
    {
        this.gameController = gameController;   
    }
    public void OnSquareSelected(Vector3 inputPosition)
    {
        if (!gameController.IsGameInProgress())
            return;
        Vector2Int coords = CalculateCoordsFromPosition(inputPosition);
        Piece piece = GetPieceOnSquare(coords);
        if (selectedPiece)
        {
            if (selectedPiece != null&&selectedPiece ==piece)
            {
                DeselectPiece();
            }
            else if(piece != null&&selectedPiece !=piece && gameController.IsTeamTurnActive(piece.team))
            {
                SelectPiece(piece);
            }
            else if (selectedPiece.CanMoveTo(coords))
            {
                OnSelectedPieceMove(coords,selectedPiece);
            }
        }
        else
        {
            if(piece != null && gameController.IsTeamTurnActive(piece.team))
            {
                SelectPiece(piece);
            }
        }
    }

    private void OnSelectedPieceMove(Vector2Int coords, Piece piece)
    {
        TryToTakeOppositepiece(coords);

        UpdateBoardOnPieceMove(coords, piece.occupiedSquare,null, piece);
        selectedPiece.MovePiece(coords);
        DeselectPiece();
        EndTurn();
    }

    private void TryToTakeOppositepiece(Vector2Int coords)
    {
        Piece piece = GetPieceOnSquare(coords);
        if (piece!=null && !selectedPiece.IsFromSameTeam(piece))
        {
            TakePiece(piece);
        }
    }

    private void TakePiece(Piece piece)
    {
        if (piece)
        {
            grid[piece.occupiedSquare.x, piece.occupiedSquare.y] = null;
            gameController.OnPieceRemoved(piece);
        }
    }

    private void EndTurn()
    {
        gameController.EndTurn();
    }

    public void UpdateBoardOnPieceMove(Vector2Int newCoords,Vector2Int oldCoords, Piece oldPiece,Piece newPiece)
    {
        grid[oldCoords.x, oldCoords.y] = oldPiece;
        grid[newCoords.x, newCoords.y] = newPiece;
    }

    private void SelectPiece(Piece piece)
    {
        gameController.RemoveMovesEnablingAttackOnPieceOfType<King>(piece);
        selectedPiece = piece;
        List<Vector2Int> selection = selectedPiece.availableMoves;
        ShowSelectionSquares(selection);
        Debug.Log($"Selected piece: {selectedPiece}");
    }

    private void ShowSelectionSquares(List<Vector2Int> selection)
    {
        Dictionary<Vector3,bool> squareData = new Dictionary<Vector3, bool> ();
        for (int i = 0; i < selection.Count; i++)
        {
            Vector3 position = CalculatePositionFromCoords(selection[i]);
            bool isSquareFree = GetPieceOnSquare(selection[i])==null;
            squareData.Add(position,isSquareFree);
        }
        squareSelectorCreator.ShowSelection(squareData);
    }

    private void DeselectPiece()
    {
        Debug.Log($"DeSelected piece: {selectedPiece}");
        selectedPiece = null;
        squareSelectorCreator.ClearSelection();
    }

    public Piece GetPieceOnSquare(Vector2Int coords)
    {
        if(CheckIfCoordinatedAreaOnBoard(coords))
        {
            return grid[coords.x, coords.y];
        }
        return null;
    }

    public bool CheckIfCoordinatedAreaOnBoard(Vector2Int coords)
    {
        if(coords.x<0||coords.y<0||coords.x>=BOARD_SIZE||coords.y>=BOARD_SIZE)
            return false;
        return true; 
    }

    private Vector2Int CalculateCoordsFromPosition(Vector3 inputPosition)
    {
        int x =Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).x/squareSize+BOARD_SIZE/2);
        int y =Mathf.FloorToInt(transform.InverseTransformPoint(inputPosition).z/squareSize+BOARD_SIZE/2);
        return new Vector2Int(x, y);
    }

    private void CreaterGrid()
    {
        grid = new Piece[BOARD_SIZE, BOARD_SIZE];
    }
    public Vector3 CalculatePositionFromCoords(Vector2Int coords)
    {
        return bottomLeftSquareTransform.position + new Vector3(coords.x * squareSize,0f,coords.y*squareSize);
    }
    public bool HasPiece(Piece piece)
    {
        for (int i = 0; i < BOARD_SIZE; i++)
        {
            for(int j = 0; j < BOARD_SIZE; j++)
            {
                if (grid[i, j] == piece)
                {
                    return true;
                }

            }
        }
        return false;
    }

    public void SetPieceOnBoard(Vector2Int coords, Piece piece)
    {
        if(CheckIfCoordinatedAreaOnBoard(coords))
            grid[coords.x,coords.y] = piece;
    }

    internal void OnGameRestarted()
    {
        selectedPiece = null;
        CreaterGrid();
    }

    public void PromotePiece(Piece piece)
    {
        TakePiece(piece);
        gameController.CreatePieceAndInitialize(piece.occupiedSquare, piece.team, typeof(Queen));
    }
}
