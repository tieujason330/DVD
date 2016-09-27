﻿using UnityEngine;
using System.Collections;

public class IdleStateBehavior : StateMachineBehaviour
{

    private PlayerCombat _playerCombat;
    public bool _hasAttacked;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _playerCombat = GameObject.FindGameObjectWithTag(Consts.TAG_PLAYER).GetComponent<PlayerCombat>();
        _hasAttacked = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_hasAttacked) return;

        if (_playerCombat._playerMain.InputAttack)
        {
            _playerCombat.MeleePressedInState(CombatState.BufferState);
            _hasAttacked = true;
        }
        else
        {
            _playerCombat.MeleeNotPressedInState(CombatState.IdleState);
        }
    }
}
