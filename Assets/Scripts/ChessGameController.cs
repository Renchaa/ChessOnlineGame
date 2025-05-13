using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(PieceCreator))]
public class ChessGameController : MonoBehaviour
{
    private enum GameState { Init,Play,Finished}

    [SerializeField] private BoardLayout staratingBoardLayout;
    [SerializeField] private Board board;
    [SerializeField] private ChessUiManager chessUiManager;
    private PieceCreator pieceCreator;

    private Chessplayer whitePlayer;
    private Chessplayer blackPlayer;
    private Chessplayer activePlayer;
    private GameState gameState;
    private void Awake()
    {
        SetDependencies();
        CreatePlayers();
    }
    private void SetDependencies()
    {
        pieceCreator = GetComponent<PieceCreator>();
    }
    private void CreatePlayers()
    {
        whitePlayer = new Chessplayer(TeamColor.White,board);
        blackPlayer = new Chessplayer(TeamColor.Black,board);
    }
    private void Start()
    {
        StartNewGame();
    }
    private void StartNewGame()
    {
        chessUiManager.HideUI();
        SetGameState(GameState.Init);
        board.SetDependencies(this);
        CreatePiecesFromLayout(staratingBoardLayout);
        activePlayer = whitePlayer;
        GenerateAllPossibleMoves(activePlayer);
        SetGameState(GameState.Play);
    }

    public void RestartGame()
    {
        DestroyAllPieces();
        board.OnGameRestarted();
        whitePlayer.OnGameRestarted();
        blackPlayer.OnGameRestarted();
        StartNewGame();
    }

    private void DestroyAllPieces()
    {
        whitePlayer.activePieces.ForEach(p=>Destroy(p.gameObject));
        blackPlayer.activePieces.ForEach(p=>Destroy(p.gameObject));
    }

    private void SetGameState(GameState state)
    {
        this.gameState = state;
    }
    public bool IsGameInProgress()
    {
        return gameState == GameState.Play;
    }
    private void CreatePiecesFromLayout(BoardLayout startingBoardLayout)
    {
        for (int i = 0;i < startingBoardLayout.GetPiecesCount(); i++)
        {
            Vector2Int squareCoords = startingBoardLayout.GetSquareCoordsAtIndex(i);
            TeamColor team = startingBoardLayout.GetTeamColorAtIndex(i);
            string typeName = startingBoardLayout.GetSquarePieceNameAtIndex(i);
            Debug.Log($"Square coord at position x: {squareCoords.x} y: {squareCoords.y} team color: {team} and typeName: {typeName}");

            Type type = Type.GetType(typeName);
            Debug.Log("type: " + type);
            CreatePieceAndInitialize(squareCoords, team, type);
        }
    }
 
    public void CreatePieceAndInitialize(Vector2Int squareCoords, TeamColor team, Type type)
    {
        Piece newPiece = pieceCreator.CreatePiece(type).GetComponent<Piece>();
        if (newPiece == null)
            Debug.Log("new piece is null");
        newPiece.SetData(squareCoords,team,board);

        Material teamMaterial = pieceCreator.GetTeamMaterial(team);
        newPiece.SetMaterial(teamMaterial);

        board.SetPieceOnBoard(squareCoords, newPiece);

        Chessplayer currentPlayer = team == TeamColor.White ? whitePlayer : blackPlayer;
        if (currentPlayer == null) Debug.Log("Player is null");
        currentPlayer.AddPiece(newPiece);
    }
    private void GenerateAllPossibleMoves(Chessplayer player)
    {
        player.GenerateAllPossibleMoves();
    }
    public bool IsTeamTurnActive(TeamColor team)
    {
        return activePlayer.team == team;
    }

    public void EndTurn()
    {
        Debug.Log("End of turn");
        GenerateAllPossibleMoves(activePlayer);
        GenerateAllPossibleMoves(GetOpponentToPlayer(activePlayer));
        if (CheckIfGameIsFinished())
            EndGame();
        else
            ChangeActiveTeam();
    }

    private void EndGame()
    {
        chessUiManager.OnGameFinished(activePlayer.team.ToString());


        Debug.Log("GameFinished");
        SetGameState(GameState.Finished);
    }
    
    private bool CheckIfGameIsFinished()
    {
        Piece[] kingAttackingPieces = activePlayer.GetPiecesAttackingOppositePieceOfType<King>();
        if(kingAttackingPieces.Length > 0)
        {
            Chessplayer oppositePlayer = GetOpponentToPlayer(activePlayer);
            Piece attackingKing = oppositePlayer.GetPiecesOfType<King>().FirstOrDefault();
            oppositePlayer.RemoveMovesEnablingAttackOnPiece<King>(activePlayer, attackingKing);

            int availableKingMoves = attackingKing.availableMoves.Count;
            if(availableKingMoves == 0)
            {
                bool canCoverKing = oppositePlayer.CanHidePieceeFromAttack<King>(activePlayer);
                if (!canCoverKing)
                    return true;
            }
        }
        return false;
    }
    private void ChangeActiveTeam()
    {
        activePlayer =activePlayer==whitePlayer?blackPlayer:whitePlayer;
    }
    private Chessplayer GetOpponentToPlayer(Chessplayer chessplayer)
    {
        return chessplayer == whitePlayer ? blackPlayer : whitePlayer;
    }

    public void RemoveMovesEnablingAttackOnPieceOfType<T>(Piece piece) where T : Piece
    {
        activePlayer.RemoveMovesEnablingAttackOnPiece<T>(GetOpponentToPlayer(activePlayer),piece);
    }

    public void OnPieceRemoved(Piece piece)
    {
        Chessplayer pieceOwner = (piece.team ==TeamColor.White) ? whitePlayer: blackPlayer;
        pieceOwner.RemovePiece(piece);
        Destroy(piece.gameObject);
    }
}
