using UnityEngine;
using System.Collections;

public class ActiveAbilityStateBehavior : StateMachineBehaviour
{

    private PlayerCharacter _player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag(Consts.TAG_PLAYER).GetComponent<PlayerCharacter>();
        _player.StopMeleeCombo(CombatState.ActiveAbilityState);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
