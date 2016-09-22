using UnityEngine;
using System.Collections;

public class BufferStateBehavior : StateMachineBehaviour
{

    private PlayerCharacter _player;
    public bool _attackButtonPressed;
    public bool _hasAttacked;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = GameObject.FindGameObjectWithTag(Consts.TAG_PLAYER).GetComponent<PlayerCharacter>();
        _hasAttacked = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_hasAttacked) return;

        _attackButtonPressed = Input.GetButtonDown("Attack");
        if (_attackButtonPressed)
        {
            _player.MeleePressedInState(MeleeState.BufferState);
            _hasAttacked = true;
        }
        else
        {
            _player.MeleeNotPressedInState();
        }
    }
}
