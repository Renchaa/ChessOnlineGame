using System;
using UnityEngine;

public class BoardInputHandler : MonoBehaviour,IInputHandler
{
    private Board board;

    private void Awake()
    {
        board = GetComponent<Board>();
    }

    public void ProcessInput(Vector3 inputPosition, GameObject selectedObject, Action callback)
    {
        board.OnSquareSelected(inputPosition);
    }
}
