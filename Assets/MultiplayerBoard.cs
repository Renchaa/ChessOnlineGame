using Photon.Pun;
using System;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class MultiplayerBoard : Board
{
    private PhotonView photonView;


    protected override void Awake()
    {
        base.Awake();
        photonView = GetComponent<PhotonView>();
    }

    public override void SelectedPieceMoved(Vector2 coords)
    {
        Vector2Int c = Vector2Int.RoundToInt(coords);
        OnSelectedPieceMove(c);
        photonView.RPC(nameof(RPC_OnSelectedPieceMoved), RpcTarget.OthersBuffered, new object[] { coords });
    }

    public override void SetSelectedPiece(Vector2 coords)
    {
        Vector2Int c = Vector2Int.RoundToInt(coords);
        OnSetSelectedPiece(c);
        photonView.RPC(nameof(RPC_SetSelectedPiece), RpcTarget.OthersBuffered, new object[] { coords });
    }


    [PunRPC]
    private void RPC_SetSelectedPiece(Vector2 coords)
    {
        Debug.LogError("ON RPC select");

        Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x), Mathf.RoundToInt(coords.y));
        OnSetSelectedPiece(intCoords);
    }

    [PunRPC]
    private void RPC_OnSelectedPieceMoved(Vector2 coords)
    {
        Debug.LogError("ON RPC move");

        Vector2Int intCoords = new Vector2Int(Mathf.RoundToInt(coords.x), Mathf.RoundToInt(coords.y));
        OnSelectedPieceMove(intCoords);
    }
}
