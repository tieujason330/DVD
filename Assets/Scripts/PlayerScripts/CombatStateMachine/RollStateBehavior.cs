using UnityEngine;
using System.Collections;

public class RollStateBehavior : StateMachineBehaviour
{
    private PlayerCharacter _player;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag(Consts.TAG_PLAYER).GetComponent<PlayerCharacter>();
        _player.StopMeleeCombo(CombatState.RollState);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
