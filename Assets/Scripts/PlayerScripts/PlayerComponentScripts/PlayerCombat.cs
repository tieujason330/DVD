using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{

    public PlayerMain _playerMain;

    private Animator _animator;
    private int _animatorAimParameter;
    private int _animatorMeleeEquippedParameter;
    private int _animatorRightArmParameter;
    private int _animatorLeftArmParameter;
    private int _animatorRightAttackComboCounterParameter;
    private int _animatorLeftAttackComboCounterParameter;
    private int _animtorDamagedParameter;
    private int _animtorRollParameter;
    private int _animtorComboTimerOnParameter;

    private int _animatorUsingActiveAbilityParameter;
    private int _animatorHeadActiveAbilityParameter;
    private int _animatorTorsoActiveAbilityParameter;
    private int _animatorRightArmActiveAbilityParameter;
    private int _animatorLeftArmActiveAbilityParameter;
    private int _animatorLegsActiveAbilityParameter;

    //public const int ATTACK_MAXIMUM_COMBO_COUNT = 6;
    //private int _attackMaxComboCount = 6;
    private int _rightComboCounter;
    private int _leftComboCounter;
    private int _attackComboPoints;
    public float _attackInitialComboTimer;
    public float _attackCurrentComboTimer;
    private ComboTimer _attackComboTimerComponent;
    private ComboPoints _attackComboPointsComponent;
    private float _attackComboTimerDuration = 1.0f;

    void Awake()
    {
        InitializePlayerLogic();
        InitializeAnimatorLogic();
        InitializeComboLogic();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlayerUpdate()
    {
        Update_ComboTimerLogic();
        CombatManagement();
    }

    #region Combat logic

    void InitializePlayerLogic()
    {
        _playerMain = gameObject.GetComponent<PlayerMain>();
    }

    void InitializeAnimatorLogic()
    {
        _animator = GetComponent<Animator>();

        _animatorAimParameter = Animator.StringToHash("Aim");
        _animatorMeleeEquippedParameter = Animator.StringToHash("MeleeEquipped");
        _animatorRightArmParameter = Animator.StringToHash("RightArm");
        _animatorLeftArmParameter = Animator.StringToHash("LeftArm");
        _animatorRightAttackComboCounterParameter = Animator.StringToHash("RightComboCounter");
        _animatorLeftAttackComboCounterParameter = Animator.StringToHash("LeftComboCounter");
        _animtorDamagedParameter = Animator.StringToHash("Damaged");
        _animtorComboTimerOnParameter = Animator.StringToHash("ComboTimerOn");
        _animtorRollParameter = Animator.StringToHash("Roll");

        _animatorUsingActiveAbilityParameter = Animator.StringToHash("UsingActiveAbility");
        _animatorHeadActiveAbilityParameter = Animator.StringToHash("HeadActiveAbility");
        _animatorTorsoActiveAbilityParameter = Animator.StringToHash("TorsoActiveAbility");
        _animatorRightArmActiveAbilityParameter = Animator.StringToHash("RightArmActiveAbility");
        _animatorLeftArmActiveAbilityParameter = Animator.StringToHash("LeftArmActiveAbility");
        _animatorLegsActiveAbilityParameter = Animator.StringToHash("LegsActiveAbility");
    }

    void InitializeComboLogic()
    {
        _attackComboTimerComponent = GetComponentInChildren<ComboTimer>();
        _attackComboTimerComponent.gameObject.SetActive(false);
        _attackComboPointsComponent = GetComponentInChildren<ComboPoints>();
        _attackCurrentComboTimer = _attackInitialComboTimer;
    }

    void Update_ComboTimerLogic()
    {
        if (_attackComboTimerComponent.gameObject.activeSelf)
            ComboTimerTick();
    }

    void CombatManagement()
    {
        if (_playerMain.InputRoll)
            _animator.SetTrigger(_animtorRollParameter);

        ActiveAbilityManagement();
    }

    void ActiveAbilityManagement()
    {
        _animator.SetBool(_animatorHeadActiveAbilityParameter, _playerMain.InputHeadActiveAbility);
        _animator.SetBool(_animatorTorsoActiveAbilityParameter, _playerMain.InputTorsoActiveAbility);
        _animator.SetBool(_animatorRightArmActiveAbilityParameter, _playerMain.InputRightArmActiveAbility);
        _animator.SetBool(_animatorLeftArmActiveAbilityParameter, _playerMain.InputLeftArmActiveAbility);
        _animator.SetBool(_animatorLegsActiveAbilityParameter, _playerMain.InputLegsActiveAbility);
        var usingAbility = _playerMain.InputHeadActiveAbility || _playerMain.InputTorsoActiveAbility ||
                           _playerMain.InputRightArmActiveAbility || _playerMain.InputLeftArmActiveAbility ||
                           _playerMain.InputLegsActiveAbility;
        _animator.SetBool(_animatorUsingActiveAbilityParameter, usingAbility);
    }

    void ComboTimerTick()
    {
        _attackCurrentComboTimer -= Time.deltaTime;
        if (_attackCurrentComboTimer < 0.0f)
        {
            DisableComboTimer();
        }
    }

    void ResetComboTimer()
    {
        if (!_attackComboTimerComponent.gameObject.activeSelf)
        {
            _attackCurrentComboTimer = _attackInitialComboTimer = _attackComboTimerDuration;
            _attackComboTimerComponent.gameObject.SetActive(true);
            _animator.SetBool(_animtorComboTimerOnParameter, true);
        }
    }

    void DisableComboTimer()
    {
        _attackCurrentComboTimer = 0.0f;
        _attackComboTimerComponent.gameObject.SetActive(false);
        _animator.SetBool(_animtorComboTimerOnParameter, false);
    }

    //public void SetupEquipmentLogic(int maxComboCount, AnimationClip[] overridedAnimations)
    //{
    //    _attackMaxComboCount = maxComboCount;
    //    OverrideAttackAnimations(overridedAnimations);
    //}

    //public void OverrideAttackAnimations(AnimationClip[] overridedAnimations = null)
    //{
    //    if (overridedAnimations == null)
    //    {
    //        //reset character animations
    //        return;
    //    }

    //    RuntimeAnimatorController runtime = _animator.runtimeAnimatorController;
    //    AnimatorOverrideController over = new AnimatorOverrideController();
    //    over.runtimeAnimatorController = runtime;

    //    string animName = "MeleeAttack0";

    //    for (int i = 0; i < overridedAnimations.Length && i < ATTACK_MAXIMUM_COMBO_COUNT; i++)
    //    {
    //        over[animName + (i + 1)] = overridedAnimations[i];
    //    }


    //    _animator.runtimeAnimatorController = over;
    //}

    public void StopMeleeCombo(CombatState state)
    {
        if (state == CombatState.RollState)
        {
            _leftComboCounter = 0;
            _rightComboCounter = 0;
            _attackComboPoints = 0;
        }
        else if (state == CombatState.ActiveAbilityState)
        {
            _playerMain.IsUsingAbility = true;
        }

        DisableComboTimer();
        UpdateAttackFields();
    }

    public void MeleeInitializeBufferTime()
    {
        ResetComboTimer();
    }

    public void MeleeNotPressedInState(CombatState state = CombatState.UndeterminedState)
    {
        //if (state == MeleeState.BufferState)
        //{
        //    ResetComboTimer(animStateInfo.length);
        //}
        if (state == CombatState.IdleState)
        {
            _leftComboCounter = 0;
            _rightComboCounter = 0;
            _attackComboPoints = 0;
        }

        UpdateAttackFields();
    }

    public void MeleePressedInState(CombatState state, string arm = "")
    {
        if (state == CombatState.AttackState)
        {
            _attackComboPoints = 0;
        }
        else if (state == CombatState.BufferState)
        {
            if (arm.Equals("RIGHT"))
            {
                if (_rightComboCounter >= _playerMain._attackMaxComboCountRightArm)
                    _rightComboCounter = 1;
                else
                    _rightComboCounter++;

                if (_attackComboPoints < _playerMain._attackMaxComboCountRightArm)
                    _attackComboPoints++;
            }

            if (arm.Equals("LEFT"))
            {
                if (_leftComboCounter >= _playerMain._attackMaxComboCountLeftArm)
                    _leftComboCounter = 1;
                else
                    _leftComboCounter++;

                if (_attackComboPoints < _playerMain._attackMaxComboCountLeftArm)
                    _attackComboPoints++;
            }

            DisableComboTimer();
        }

        UpdateAttackFields();
    }

    public void UpdateAttackFields()
    {
        _attackComboPointsComponent.SetComboPoints(_attackComboPoints, _playerMain._attackMaxComboCountRightArm);

        _animator.SetInteger(_animatorRightAttackComboCounterParameter, _rightComboCounter);
        _animator.SetInteger(_animatorLeftAttackComboCounterParameter, _leftComboCounter);
        _animator.SetBool(_animatorRightArmParameter, !_playerMain.IsUsingAbility && _playerMain.InputRightArm);
        _animator.SetBool(_animatorLeftArmParameter, !_playerMain.IsUsingAbility && _playerMain.InputLeftArm);
    }

    public void GiveDamage(float damage, BaseWorldCharacter attackedCharacter)
    {
        if (attackedCharacter != null)
            attackedCharacter.TakeDamage(damage);
    }

    public void TakeDamage(float damage)
    {
        _animator.SetTrigger(_animtorDamagedParameter);
        if (_playerMain._currentHealth > damage)
        {
            _playerMain._currentHealth -= damage;
        }
        else
        {
            _playerMain._currentHealth = 0;
        }
        Debug.Log(string.Format("{0} ==> {1}HP", gameObject.name, _playerMain._currentHealth));
    }

    #endregion
}
