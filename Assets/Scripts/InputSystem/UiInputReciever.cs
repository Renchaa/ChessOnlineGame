using UnityEngine;
using UnityEngine.Events;

public class UiInputReciever : InputReciever
{
    [SerializeField] private UnityEvent clickEvent;
    public override void OnInputRecieved()
    {
        foreach (var handler in inputHandler)
        {
            handler.ProcessInput(Input.mousePosition, gameObject, () => clickEvent.Invoke());
        }
    }
}
