using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{

    public PlayerMain _playerMain;

    private Animator _animator;
    private int _animatorAimParameter;
    private int _animatorMeleeEquippedParameter;
    private int _animatorMeleeAttackParameter;
    private int _animtorAttackComboCounterParameter;
    private int _animtorDamagedParameter;
    private int _animtorRollParameter;
    private int _animtorComboTimerOnParameter;
    private int _animatorActiveAbilityParameter;

    private const int ATTACK_MAXIMUM_COMBO_COUNT = 6;
    private int _attackMaxComboCount = 6;
    private int _attackComboCounter;
    private int _attackComboPoints;
    public float _attackInitialComboTimer;
    public float _attackCurrentComboTimer;
    private ComboTimer _attackComboTimerComponent;
    private ComboPoints _attackComboPointsComponent;
    private float _attackComboTimerDuration = 0.5f;
    public bool IsAttacking { get; set; }
    public bool IsUsingAbility { get; set; }

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
        _animatorMeleeAttackParameter = Animator.StringToHash("MeleeAttack");
        _animtorAttackComboCounterParameter = Animator.StringToHash("AttackComboCounter");
        _animtorDamagedParameter = Animator.StringToHash("Damaged");
        _animtorComboTimerOnParameter = Animator.StringToHash("ComboTimerOn");
        _animtorRollParameter = Animator.StringToHash("Roll");
        _animatorActiveAbilityParameter = Animator.StringToHash("ActiveAbility");
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
        bool performAbility = false;

        if (_playerMain.InputActiveAbility01 && _attackComboPoints >= 2)
        {
            _attackComboPoints -= 2;
            performAbility = true;
        }
        _animator.SetBool(_animatorActiveAbilityParameter, performAbility);
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

    public void SetupEquipmentLogic(int maxComboCount, AnimationClip[] overridedAnimations)
    {
        _attackMaxComboCount = maxComboCount;
        OverrideAttackAnimations(overridedAnimations);
    }

    public void OverrideAttackAnimations(AnimationClip[] overridedAnimations = null)
    {
        if (overridedAnimations == null)
        {
            //reset character animations
            return;
        }

        RuntimeAnimatorController runtime = _animator.runtimeAnimatorController;
        AnimatorOverrideController over = new AnimatorOverrideController();
        over.runtimeAnimatorController = runtime;

        string animName = "MeleeAttack0";

        for (int i = 0; i < overridedAnimations.Length && i < ATTACK_MAXIMUM_COMBO_COUNT; i++)
        {
            over[animName + (i + 1)] = overridedAnimations[i];
        }


        _animator.runtimeAnimatorController = over;
    }

    public void StopMeleeCombo(CombatState state)
    {
        if (state == CombatState.RollState)
        {
            _attackComboCounter = 0;
            _attackComboPoints = 0;
        }
        else if (state == CombatState.ActiveAbilityState)
        {
            IsUsingAbility = true;
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
            _attackComboCounter = 0;
            _attackComboPoints = 0;
        }

        UpdateAttackFields();
    }

    public void MeleePressedInState(CombatState state)
    {
        if (state == CombatState.AttackState)
        {
            _attackComboPoints = 0;
        }
        else if (state == CombatState.BufferState)
        {
            if (_attackComboCounter >= _attackMaxComboCount)
                _attackComboCounter = 1;
            else
                _attackComboCounter++;

            if (_attackComboPoints < _attackMaxComboCount)
                _attackComboPoints++;

            DisableComboTimer();
        }

        UpdateAttackFields();
    }

    public void UpdateAttackFields()
    {
        _attackComboPointsComponent.SetComboPoints(_attackComboPoints, _attackMaxComboCount);

        _animator.SetInteger(_animtorAttackComboCounterParameter, _attackComboCounter);
        _animator.SetBool(_animatorMeleeAttackParameter, !IsUsingAbility && _playerMain.InputAttack);
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
