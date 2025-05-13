using System;
using UnityEngine;

public interface IInputHandler 
{
    void ProcessInput(Vector3 inputPosition,GameObject selectObject,Action callback);
}
