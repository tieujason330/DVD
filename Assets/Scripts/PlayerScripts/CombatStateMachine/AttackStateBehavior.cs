using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;

public class AttackStateBehavior : StateMachineBehaviour
{

    private PlayerCharacter _player;
    public bool _hasAttacked;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag(Consts.TAG_PLAYER).GetComponent<PlayerCharacter>();
        _hasAttacked = false;
        _player.SetIsAttacking(true);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_hasAttacked) return;
        
        if (_player._inputAttack)
        {
            _player.MeleePressedInState(CombatState.AttackState);
            _hasAttacked = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player.SetIsAttacking(false);
    }
}
