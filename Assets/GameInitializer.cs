using Photon.Pun;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [Header("Game mode dpendent objects")]
    [SerializeField] private SinglePlayerChessGameController singlePlayerChessGameController;
    [SerializeField] private MultiplayerChessGameController multiplayerChessGameController;
    [SerializeField] private SingleplayerBoard singleplayerBoard;
    [SerializeField] private MultiplayerBoard multiplayerBoard;

    [Header("SceneReference")]
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private ChessUiManager uiManager;
    [SerializeField] private Transform boardAnchor;
    [SerializeField] private CameraSetup cameraSetUp;

    public void CreateMultiplayerBoard()
    {
        if(!networkManager.IsRoomFull())
            PhotonNetwork.Instantiate(multiplayerBoard.name,boardAnchor.position,boardAnchor.rotation);
    }
    public void CreateSingleplayerBoard()
    {
        Instantiate(singleplayerBoard, boardAnchor);
    }
    public void InitializeMulitplayerController()
    {
        MultiplayerBoard board = FindAnyObjectByType<MultiplayerBoard>();
        if (board)
        {
            MultiplayerChessGameController controller = Instantiate(multiplayerChessGameController);
            controller.SetDependencies(uiManager,board,cameraSetUp);
            controller.CreatePlayers();
            controller.SetMultiplayerDependencies(networkManager);
            networkManager.SetDependecies(controller);
            board.SetDependencies(controller);
        }
    }
    public void InitializeSinglePlayerController()
    {
        SingleplayerBoard board = FindAnyObjectByType<SingleplayerBoard>();
        if (board)
        {
            SinglePlayerChessGameController controller = Instantiate(singlePlayerChessGameController);
            controller.SetDependencies(uiManager, board, cameraSetUp);
            controller.CreatePlayers();
            board.SetDependencies(controller);
            controller.StartNewGame();
        }
    }
}

