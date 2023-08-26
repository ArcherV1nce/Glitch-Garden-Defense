using UnityEngine;

[RequireComponent(typeof(Defender))]
public class DefenderAnimationControl : AnimationControl
{
    public void UpdateStates(DefenderState newState)
    {
        foreach (DefenderStateParameter stateParameter in newState.Parameters)
        {
            Animator.SetBool(stateParameter.Name.ToString(), stateParameter.State);
        }
    }
}
