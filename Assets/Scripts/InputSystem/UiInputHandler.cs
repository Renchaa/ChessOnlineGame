using System;
using UnityEngine;
using UnityEngine.Events;

public class UiInputHandler : MonoBehaviour, IInputHandler
{
    public void ProcessInput(Vector3 inputPosition,GameObject selectedObject,Action callback)
    {
        callback?.Invoke();
    }
}
