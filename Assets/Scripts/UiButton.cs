using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UiInputReciever))]
public class UiButton : Button
{
    private InputReciever reciever;
    protected override void Awake()
    {
        base.Awake();

        reciever = GetComponent<InputReciever>();
        onClick.AddListener(()=>reciever.OnInputRecieved());
    }
}
