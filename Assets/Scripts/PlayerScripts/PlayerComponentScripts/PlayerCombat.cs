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

    public float _attackInitialComboTimer;
    public float _attackCurrentComboTimer;
    private CombatTimer _attackComboTimerComponent;
    public float _attackComboTimerDuration = 0.5f;

    public float _staminaRegenerationSpeed = 10.0f;

    //public float _potentialStaminaRegain = 0.0f;
    private float _potentialCombatPointsGain = 0.0f;
    private float tempRightStaminaCost = 10.0f;
    private float tempLeftStaminaCost = 20.0f;
    private bool _rightArmAttack = false;
    private bool _leftArmAttack = false;

    private CombatState _combatState = CombatState.UndeterminedState;

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
        //Update_ComboTimerLogic();
        //CombatManagement();
    }

    /// <summary>
    /// Called by PlayerMain update
    /// </summary>
    public void PlayerUpdate()
    {
        Update_ComboTimerLogic();
        CombatManagement();
    }

    #region initialize logic

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
        _attackComboTimerComponent = GetComponentInChildren<CombatTimer>();
        _attackComboTimerComponent.gameObject.SetActive(false);
        _attackCurrentComboTimer = _attackInitialComboTimer;
    }

    #endregion

    #region update logic

    void Update_ComboTimerLogic()
    {
        if (_attackComboTimerComponent.gameObject.activeSelf)
            ComboTimerTick();
    }

    void CombatManagement()
    {
        if (!_animator.isInitialized) return;

        MeleeAttackManagement();
        RollManagement();
        ActiveAbilityManagement();
    }

    void RollManagement()
    {
        if (_playerMain.InputRoll && !_playerMain.IsUsingAbility)
        {
            if (_playerMain._currentStamina > 0.0f)
            {
                StaminaCostLogic(30.0f);
                _animator.SetTrigger(_animtorRollParameter);
            }
        }
    }

    //need to replace temp costs with weapon costs
    void MeleeAttackManagement()
    {
        float costMultiplier = 1.0f;

        if (_combatState == CombatState.RollState || _combatState == CombatState.ActiveAbilityState)
            return;

        //if trying to spam attack during attack anims, punish w/ stamina cost
        if (_combatState == CombatState.AttackState)
            costMultiplier = 0.5f;

        if (_playerMain.InputRightArm)
        {
            if (_playerMain._currentStamina > 0.0f)
            {
                StaminaCostLogic(tempRightStaminaCost * costMultiplier);
                _rightArmAttack = true;
            }
            else
                _rightArmAttack = false;
        }
        else if (_playerMain.InputLeftArm)
        {
            if (_playerMain._currentStamina > 0.0f)
            {
                StaminaCostLogic(tempLeftStaminaCost * costMultiplier);
                _leftArmAttack = true;
            }
            else
                _leftArmAttack = false;
        }
        else
        {
            _rightArmAttack = _leftArmAttack = false;
        }
    }

    void ActiveAbilityManagement()
    {
        if (_playerMain._headActiveAbility != null)
        {
            _animator.SetBool(_animatorHeadActiveAbilityParameter, _playerMain.InputHeadActiveAbility);
            if (_playerMain.InputHeadActiveAbility)
            {
                _playerMain._headActiveAbility.Execute();
            }
        }

        if (_playerMain._torsoActiveAbility != null)
        {
            _animator.SetBool(_animatorTorsoActiveAbilityParameter, _playerMain.InputTorsoActiveAbility);
            if (_playerMain.InputTorsoActiveAbility)
            {
                _playerMain._torsoActiveAbility.Execute();
            }
        }

        if (_playerMain._rightArmActiveAbility != null)
        {
            _animator.SetBool(_animatorRightArmActiveAbilityParameter, _playerMain.InputRightArmActiveAbility);
            if (_playerMain.InputRightArmActiveAbility)
            {
                _playerMain._rightArmActiveAbility.Execute();
            }
        }

        if (_playerMain._leftArmActiveAbility != null)
        {
            _animator.SetBool(_animatorLeftArmActiveAbilityParameter, _playerMain.InputLeftArmActiveAbility);
            if (_playerMain.InputLeftArmActiveAbility)
            {
                _playerMain._leftArmActiveAbility.Execute();
            }
        }

        if (_playerMain._legsActiveAbility != null)
        {
            _animator.SetBool(_animatorLegsActiveAbilityParameter, _playerMain.InputLegsActiveAbility);
            if (_playerMain.InputLegsActiveAbility)
            {
                _playerMain._legsActiveAbility.Execute();
            }
        }

        _animator.SetBool(_animatorUsingActiveAbilityParameter, _playerMain.IsUsingAbility);
    }

    #endregion

    #region combo timer

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

    #endregion

    #region stamina logic

    void StaminaRegeneration(float speed)
    {
        if (_playerMain._currentStamina < _playerMain._initialStamina)
            _playerMain._currentStamina += Time.deltaTime * speed;
    }

    void StaminaCostLogic(float staminaCost)
    {
        var combatPointsGain = 0.0f;
        if (_playerMain._currentStamina >= staminaCost)
        {
            _playerMain._currentStamina -= staminaCost;
            _playerMain._potentialStaminaRegain += staminaCost;

            combatPointsGain = staminaCost;
        }
        else
        {
            float previousStamina = _playerMain._currentStamina;
            _playerMain._currentStamina = 0.0f;
            _playerMain._potentialStaminaRegain += previousStamina;

            combatPointsGain = previousStamina;
        }

        _potentialCombatPointsGain = combatPointsGain;
    }

    public void StaminaGainBack()
    {
        _playerMain._currentStamina += _playerMain._potentialStaminaRegain;
        _playerMain._potentialStaminaRegain = 0.0f;
    }

    #endregion

    #region combat points logic

    private void IncreaseCombatPoints()
    {
        _playerMain._currentCombatPoints += _potentialCombatPointsGain;
        if (_playerMain._currentCombatPoints > _playerMain._initialCombatPoints)
            _playerMain._currentCombatPoints = _playerMain._initialCombatPoints;
    }

    private void CombatPointsDrain(float speed)
    {
        if (_playerMain._currentCombatPoints > 0.0f)
            _playerMain._currentCombatPoints -= Time.deltaTime * speed;

        if (_playerMain._currentCombatPoints < 0.0f)
            _playerMain._currentCombatPoints = 0.0f;
    }

    #endregion

    #region state machine logic

    public void SetCombatState(CombatState combatState)
    {
        _combatState = combatState;

        if (_combatState == CombatState.BufferState)
        {
            MeleeInitializeBufferTime();
            UpdateAttackFields();
        }
        else if (_combatState == CombatState.ActiveAbilityState || _combatState == CombatState.RollState)
        {
            StopMeleeCombo();
        }
    }

    private void StopMeleeCombo()
    {
        if (_combatState == CombatState.RollState)
        {
            _leftComboCounter = 0;
            _rightComboCounter = 0;
        }
        else if (_combatState == CombatState.ActiveAbilityState)
        {
            _playerMain.IsUsingAbility = true;
        }

        DisableComboTimer();
        UpdateAttackFields();
    }

    private void MeleeInitializeBufferTime()
    {
        ResetComboTimer();
    }

    public void MeleeNotPressedInState()
    {
        if (_combatState == CombatState.BufferState)
        {
            //StaminaRegeneration(_staminaRegenerationSpeed / 2.0f);
        }
        else if (_combatState == CombatState.IdleState)
        {
            _leftComboCounter = 0;
            _rightComboCounter = 0;

            _playerMain._potentialStaminaRegain = 0.0f;
            StaminaRegeneration(_staminaRegenerationSpeed);
            CombatPointsDrain(_staminaRegenerationSpeed * 0.5f);
        }
        else if (_combatState == CombatState.RollState)
        {
            CombatPointsDrain(_staminaRegenerationSpeed * 0.5f);
        }

        UpdateAttackFields();
    }

    public void MeleePressedInState(string arm = "")
    {
        if (_combatState == CombatState.AttackState)
        {
            _playerMain._potentialStaminaRegain = 0.0f;
        }
        else if (_combatState == CombatState.BufferState || _combatState == CombatState.IdleState)
        {
            if (arm.Equals("RIGHT"))
            {
                if (_rightComboCounter >= _playerMain._attackMaxComboCountRightArm)
                    _rightComboCounter = 1;
                else
                    _rightComboCounter++;
            }

            if (arm.Equals("LEFT"))
            {
                if (_leftComboCounter >= _playerMain._attackMaxComboCountLeftArm)
                    _leftComboCounter = 1;
                else
                    _leftComboCounter++;
            }
            DisableComboTimer();
        }

        UpdateAttackFields();
    }

    private void UpdateAttackFields()
    {
        _animator.SetInteger(_animatorRightAttackComboCounterParameter, _rightComboCounter);
        _animator.SetInteger(_animatorLeftAttackComboCounterParameter, _leftComboCounter);

        _animator.SetBool(_animatorRightArmParameter, !_playerMain.IsUsingAbility && _rightArmAttack);
        _animator.SetBool(_animatorLeftArmParameter, !_playerMain.IsUsingAbility && _leftArmAttack);
    }

    #endregion

    #region damage logic

    public bool GiveDamage(float damage, BaseWorldCharacter attackedCharacter)
    {
        if (attackedCharacter != null)
        {
            if (attackedCharacter.TakeDamage(damage))
            {
                IncreaseCombatPoints();
                return true;
            }
        }
        return true;
    }

    public bool TakeDamage(float damage)
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
        return true;
    }

    #endregion

}
