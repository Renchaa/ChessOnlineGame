using UnityEngine;

public class ColliderInputRecieveer : InputReciever
{
    private Vector3 clickPosition;
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                clickPosition = hit.point;
                OnInputRecieved();
            }
        }
    }
    public override void OnInputRecieved()
    {
        foreach (var handler in inputHandler)
        {
            handler.ProcessInput(clickPosition, null, null);
        }
    }
}
