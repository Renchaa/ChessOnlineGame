using UnityEngine;

public abstract class InputReciever : MonoBehaviour
{
    protected IInputHandler[] inputHandler;

    public abstract void OnInputRecieved();

    private void Awake()
    {
        inputHandler = GetComponents<IInputHandler>();
    }
}
