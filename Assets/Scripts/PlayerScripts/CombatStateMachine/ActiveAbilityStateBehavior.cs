using UnityEngine;
using System.Collections;

public class ActiveAbilityStateBehavior : StateMachineBehaviour
{

    private PlayerCombat _playerCombat;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombat = GameObject.FindGameObjectWithTag(Consts.TAG_PLAYER).GetComponent<PlayerCombat>();
        _playerCombat.StopMeleeCombo(CombatState.ActiveAbilityState);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombat.IsUsingAbility = false;
    }
}
