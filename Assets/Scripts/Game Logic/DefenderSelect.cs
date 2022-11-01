using UnityEngine;
using UnityEngine.Events;

public class DefenderSelect : MonoBehaviour
{
    public UnityEvent<Defender> DefenderSelected;

    public void SetDefender(Defender defender)
    {
        DefenderSelected?.Invoke(defender);
    }
}