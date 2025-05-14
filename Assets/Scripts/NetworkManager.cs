using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private const string LEVEL = "level";
    private const string TEAM ="team";
    private const int MAX_PLAYERS = 2;
    [SerializeField] private ChessUiManager uiManager;
    [SerializeField] private GameInitializer gameInitializer;
    private ChessLevel playerLevel;
    private MultiplayerChessGameController chessGameController;
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Update()
    {
        uiManager.SetConnectionStatus(PhotonNetwork.NetworkClientState.ToString());
    }
    public void SetDependecies(MultiplayerChessGameController chessGameController)
    {
        this.chessGameController = chessGameController;
    }
    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.LogError($"Connected to serve. Looking for random room{playerLevel}");
            PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable() { { LEVEL, playerLevel } },MAX_PLAYERS);
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    #region Photon Callbacks
    public override void OnConnectedToMaster()
    {
        Debug.LogError($"Connected to serve. Looking for random room{playerLevel}");
        PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable() { { LEVEL, playerLevel } },MAX_PLAYERS);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.LogError($"Joining random room failed because of {message}. Creating a new one with player level {playerLevel}.");
        PhotonNetwork.CreateRoom(null, new RoomOptions
        {
            CustomRoomPropertiesForLobby = new string[] { LEVEL },
            MaxPlayers = MAX_PLAYERS,
             CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { LEVEL, playerLevel } }
        }) ;
    }
    public override void OnJoinedRoom()
    {
        Debug.LogError($"Player {PhotonNetwork.LocalPlayer.ActorNumber} joined the room with level {(ChessLevel)PhotonNetwork.CurrentRoom.CustomProperties[LEVEL]}");
        gameInitializer.CreateMultiplayerBoard();
        PrepareTeamSelectionOptions();
        uiManager.ShowTeamSelectionScreen();
    }

    private void PrepareTeamSelectionOptions()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            var firstPlayer = PhotonNetwork.CurrentRoom.GetPlayer(1);
            if (firstPlayer.CustomProperties.ContainsKey(TEAM))
            {
                var occupiedTeam = firstPlayer.CustomProperties[TEAM];
                uiManager.RestrictTeamChoice((TeamColor) occupiedTeam);
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogError($"Player {newPlayer.ActorNumber} entered the room");
    }
    public void SetPlayerLevel(ChessLevel level)
    {
        playerLevel = level;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { LEVEL, level } } );
    }

    public void SelectTeam(int team)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable {{ TEAM, team }} );
        gameInitializer.InitializeMulitplayerController();
        chessGameController.SetLocalPlayer((TeamColor) team);
        chessGameController.StartNewGame();
        chessGameController.SetUpCamera((TeamColor) team);
    }

    public bool IsRoomFull()
    {
        return PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;
    }
    #endregion
}
