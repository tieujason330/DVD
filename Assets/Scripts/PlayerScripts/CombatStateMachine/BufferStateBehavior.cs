using UnityEngine;
using System.Collections;

public class BufferStateBehavior : StateMachineBehaviour
{

    private PlayerCombat _playerCombat;
    private bool _hasAttacked;
    private bool _leftInput;
    private bool _rightInput;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombat = GameObject.FindGameObjectWithTag(Consts.TAG_PLAYER).GetComponent<PlayerCombat>();
        _hasAttacked = false;

        _playerCombat.MeleeInitializeBufferTime();
        _playerCombat.UpdateAttackFields();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_hasAttacked) return;

        _leftInput = _playerCombat._playerMain.InputLeftArm;
        _rightInput = _playerCombat._playerMain.InputRightArm;

        if (_rightInput || _leftInput)
        {
            string arm = string.Empty;
            if (_rightInput)
                arm = "RIGHT";
            else if (_leftInput)
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
