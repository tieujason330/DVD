using UnityEngine;
using System.Collections;

public class BufferStateBehavior : StateMachineBehaviour
{

    private PlayerCombat _playerCombat;
    private bool _hasAttacked;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombat = GameObject.FindGameObjectWithTag(Consts.TAG_PLAYER).GetComponent<PlayerCombat>();
        _hasAttacked = false;

        if (stateInfo.IsName("MeleeBuffer"))
            _playerCombat.MeleeInitializeBufferTime();
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
            _playerCombat.MeleePressedInState(CombatState.BufferState, arm);
            _hasAttacked = true;
        }
        else
        {
            _playerCombat.MeleeNotPressedInState(CombatState.BufferState);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
