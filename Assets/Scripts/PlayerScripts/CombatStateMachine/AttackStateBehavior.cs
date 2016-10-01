using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.Director;

public class AttackStateBehavior : StateMachineBehaviour
{

    private PlayerCombat _playerCombat;
    private bool _hasAttacked;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombat = GameObject.FindGameObjectWithTag(Consts.TAG_PLAYER).GetComponent<PlayerCombat>();
        _hasAttacked = false;
        _playerCombat._playerMain.IsAttacking = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_hasAttacked) return;
        
        if (_playerCombat._playerMain.InputRightArm || _playerCombat._playerMain.InputLeftArm)
        {
            _playerCombat.MeleePressedInState(CombatState.AttackState);
            _hasAttacked = true;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombat._playerMain.IsAttacking = false;
    }
}
