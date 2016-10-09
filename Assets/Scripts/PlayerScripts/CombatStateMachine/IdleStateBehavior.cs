using UnityEngine;
using System.Collections;

public class IdleStateBehavior : StateMachineBehaviour
{

    private PlayerCombat _playerCombat;
    private bool _hasAttacked;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombat = GameObject.FindGameObjectWithTag(Consts.TAG_PLAYER).GetComponent<PlayerCombat>();
        _hasAttacked = false;
        _playerCombat.SetCombatState(CombatState.IdleState);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_hasAttacked) return;

        var leftInput = _playerCombat._playerMain.InputLeftArm;
        var rightInput = _playerCombat._playerMain.InputRightArm;

        if (rightInput || leftInput)
        {
            string arm = string.Empty;
            if (rightInput)
                arm = "RIGHT";
            else if (leftInput)
                arm = "LEFT";
            _playerCombat.MeleePressedInState(arm);
            _hasAttacked = true;
        }
        else
        {
            _playerCombat.MeleeNotPressedInState();
        }
    }
}
