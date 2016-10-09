using UnityEngine;
using System.Collections;

public class RollStateBehavior : StateMachineBehaviour
{
    private PlayerCombat _playerCombat;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombat = GameObject.FindGameObjectWithTag(Consts.TAG_PLAYER).GetComponent<PlayerCombat>();
        _playerCombat.SetCombatState(CombatState.RollState);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombat.MeleeNotPressedInState();
    }
}
