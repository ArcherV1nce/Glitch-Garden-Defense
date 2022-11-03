using UnityEngine;
using UnityEngine.Events;

public class CellSelect : MonoBehaviour
{
    public UnityAction<Cell> CellClicked;

    private void Update()
    {
        CheckMouseInput();
    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
        }
    }
}