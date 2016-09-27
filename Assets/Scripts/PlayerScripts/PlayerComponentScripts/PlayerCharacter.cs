using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCharacter : BaseWorldCharacter
{
    public float _movementWalkSpeed = 0.15f;
	public float _movementRunSpeed = 1.0f;
	public float _movementSprintSpeed = 2.0f;
	public float _movementFlySpeed = 4.0f;
	public float _movementTurnSmoothing = 3.0f;
	public float _movementAimTurnSmoothing = 15.0f;
	public float _movementSpeedDampTime = 0.1f;
	public float _movementJumpHeight = 5.0f;
	public float _movementJumpCooldown = 1.0f;

	private float _movementTimeToNextJump = 0;
    private float _movementSpeed;
	private Vector3 _movementLastDirection;
    private bool _movementFly = false;
    private float _movementDistanceToGround;
    private float _movementSprintFactor;

    private Animator _animator;
	private int _animatorSpeedParameter;
	private int _animatorJumpParameter;
	private int _animatorHorizontalParameter;
	private int _animatorVerticalParameter;
	private int _animatorAimParameter;
	private int _animatorFlyParameter;
	private int _animtorGroundedParameter;
    private int _animatorMeleeEquippedParameter;
    private int _animatorMeleeAttackParameter;
    private int _animtorAttackComboCounterParameter;
    private int _animtorDamagedParameter;
    private int _animtorRollParameter;
    private int _animtorComboTimerOnParameter;
    private int _animatorActiveAbilityParameter;

	private float _inputHorizontal;
	private float _inputVertical;
	private bool _inputAim;
	private bool _inputRun;
	private bool _inputSprint;
    private bool _inputRoll;
    public bool _inputAttack;
    private bool _inputSelectActiveAbility;
    private bool _inputActiveAbility01;


    private const int ATTACK_MAXIMUM_COMBO_COUNT = 6;
    private int _attackMaxComboCount = 6;
    private int _attackComboCounter;
    private int _attackComboPoints;
    public float _attackInitialComboTimer;
    public float _attackCurrentComboTimer;
    private ComboTimer _attackComboTimerComponent;
    private ComboPoints _attackComboPointsComponent;
    private float _attackComboTimerDuration = 0.5f;
    //public bool IsAttacking { get; set; } 
    public bool IsUsingAbility { get; set; }

    private Transform _cameraTransform;

    private Dictionary<EquipmentComponent, BaseEquipment> _equipment;

	void Awake()
	{
		_cameraTransform = Camera.main.transform;
	   
	    InitializeAnimatorLogic();
	    InitializeComboLogic();
	    InitializePlayerLogic();
	}

    void Start()
    {
    }

	void Update()
	{
        base.Update();
		
        Update_InputLogic();
        Update_AnimatorLogic();
	    Update_ComboTimerLogic();
	}

    #region Initialization

    void InitializeAnimatorLogic()
    {
        _animator = GetComponent<Animator>();

        _animatorSpeedParameter = Animator.StringToHash("Speed");
        _animatorJumpParameter = Animator.StringToHash("Jump");
        _animatorHorizontalParameter = Animator.StringToHash("H");
        _animatorVerticalParameter = Animator.StringToHash("V");
        _animatorAimParameter = Animator.StringToHash("Aim");
        _animatorMeleeEquippedParameter = Animator.StringToHash("MeleeEquipped");
        _animatorMeleeAttackParameter = Animator.StringToHash("MeleeAttack");
        _animtorAttackComboCounterParameter = Animator.StringToHash("AttackComboCounter");
        _animtorDamagedParameter = Animator.StringToHash("Damaged");
        _animtorComboTimerOnParameter = Animator.StringToHash("ComboTimerOn");
        _animtorRollParameter = Animator.StringToHash("Roll");
        _animatorFlyParameter = Animator.StringToHash("Fly");
        _animtorGroundedParameter = Animator.StringToHash("Grounded");
        _animatorActiveAbilityParameter = Animator.StringToHash("ActiveAbility");
    }

    void InitializeComboLogic()
    {
        _attackComboTimerComponent = GetComponentInChildren<ComboTimer>();
        _attackComboTimerComponent.gameObject.SetActive(false);
        _attackComboPointsComponent = GetComponentInChildren<ComboPoints>();
        _attackCurrentComboTimer = _attackInitialComboTimer;
    }

    void InitializePlayerLogic()
    {

        _movementDistanceToGround = GetComponent<Collider>().bounds.extents.y;
        _movementSprintFactor = _movementSprintSpeed / _movementRunSpeed;
        _currentHealth = _initialHealth;
        _faction = Faction.Player;
        _status = CharacterStatus.Alive;
    }

    #endregion

    #region Input logic

    void Update_InputLogic()
    {
        // fly
        if (Input.GetButtonDown("Fly"))
            _movementFly = !_movementFly;
        _inputAim = Input.GetButton("Aim");
        _inputHorizontal = Input.GetAxis("MoveHorizontal");
        _inputVertical = Input.GetAxis("MoveVertical");
        _inputAttack = Input.GetButtonDown("Attack");
        _inputRun = Input.GetButton("Run");
        _inputSprint = Input.GetButton("Sprint");
        _inputRoll = Input.GetButtonDown("Roll");
        _inputSelectActiveAbility = Input.GetButton("SelectActiveAbility");
        _inputActiveAbility01 = Input.GetButtonDown("ActiveAbility01");
    }

    #endregion

    #region Animator logic

    void Update_AnimatorLogic()
    {
        _animator.SetBool(_animatorAimParameter, IsAiming());
        _animator.SetFloat(_animatorHorizontalParameter, _inputHorizontal);
        _animator.SetFloat(_animatorVerticalParameter, _inputVertical);
        _animator.SetBool(_animatorFlyParameter, _movementFly);
        GetComponent<Rigidbody>().useGravity = !_movementFly;
        _animator.SetBool(_animtorGroundedParameter, IsGrounded());

        if (_movementFly)
            FlyManagement(_inputHorizontal, _inputVertical);
        else
        {
            MovementManagement(_inputHorizontal, _inputVertical, true, _inputSprint);
            JumpManagement();
            CombatManagement();
            //OverrideAnimationTest();
        }
    }

    #endregion

    #region Combat logic

    void Update_ComboTimerLogic()
    {
        if (_attackComboTimerComponent.gameObject.activeSelf)
            ComboTimerTick();
    }

    void CombatManagement()
    {
        if(_inputRoll)
            _animator.SetTrigger(_animtorRollParameter);

        ActiveAbilityManagement();
    }

    void ActiveAbilityManagement()
    {
        bool performAbility = false;

        if (_inputSelectActiveAbility)
        {
            if (_inputActiveAbility01 && _attackComboPoints >= 2)
            {
                _inputAttack = false;
                _attackComboPoints -= 2;
                performAbility = true;
            }
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

    public override void SetupEquipmentLogic(int maxComboCount, AnimationClip[] overridedAnimations)
    {
        _attackMaxComboCount = maxComboCount;
        OverrideAttackAnimations(overridedAnimations);
    }

    public override void OverrideAttackAnimations(AnimationClip[] overridedAnimations = null)
    {
        if (overridedAnimations == null)
        {
            //reset character animations
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
        _animator.SetBool(_animatorMeleeAttackParameter, _inputAttack);
    }

    public override void GiveDamage(float damage, BaseWorldCharacter attackedCharacter)
    {
        if (attackedCharacter != null)
            attackedCharacter.TakeDamage(damage);
    }

    public override void TakeDamage(float damage)
    {
        _animator.SetTrigger(_animtorDamagedParameter);
        if (_currentHealth > damage)
        {
            _currentHealth -= damage;
        }
        else
        {
            _currentHealth = 0;
        }
        Debug.Log(string.Format("{0} ==> {1}HP", gameObject.name, _currentHealth));
    }

    #endregion

    #region Ability logic

    #endregion

    #region Player movement logic

    // fly
    void FlyManagement(float horizontal, float vertical)
    {
        Vector3 direction = Rotating(horizontal, vertical);
        GetComponent<Rigidbody>().AddForce(direction * _movementFlySpeed * 100 * (_inputSprint ? _movementSprintFactor : 1));
    }

    void JumpManagement()
    {
        if (GetComponent<Rigidbody>().velocity.y < 10) // already jumped
        {
            _animator.SetBool(_animatorJumpParameter, false);
            if (_movementTimeToNextJump > 0)
                _movementTimeToNextJump -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump"))
        {
            _animator.SetBool(_animatorJumpParameter, true);
            if (_movementSpeed > 0 && _movementTimeToNextJump <= 0 && !_inputAim)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, _movementJumpHeight, 0);
                _movementTimeToNextJump = _movementJumpCooldown;
            }
        }
    }

    void MovementManagement(float horizontal, float vertical, bool running, bool sprinting)
    {
        if (IsAttacking) return;

		Rotating(-vertical, -horizontal);

		if(IsMoving())
		{
			if(sprinting)
			{
				_movementSpeed = _movementSprintSpeed;
			}
			else if (running)
			{
				_movementSpeed = _movementRunSpeed;
			}
			else
			{
				_movementSpeed = _movementWalkSpeed;
			}

			_animator.SetFloat(_animatorSpeedParameter, _movementSpeed, _movementSpeedDampTime, Time.deltaTime);
		}
		else
		{
			_movementSpeed = 0f;
			_animator.SetFloat(_animatorSpeedParameter, 0f);
		}
		GetComponent<Rigidbody>().AddForce(Vector3.forward*_movementSpeed);
	}

	Vector3 Rotating(float horizontal, float vertical)
	{
        Vector3 forward = _cameraTransform.TransformDirection(Vector3.forward);
        if (!_movementFly)
            forward.y = 0.0f;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        Vector3 targetDirection;

        float finalTurnSmoothing;

        if (IsAiming())
        {
            targetDirection = forward;
            finalTurnSmoothing = _movementAimTurnSmoothing;
        }
        else
        {
            targetDirection = forward * vertical + right * horizontal;
            finalTurnSmoothing = _movementTurnSmoothing;
        }

        //if ((_isMoving && targetDirection != Vector3.zero) || IsAiming())
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        //    // fly
        //    if (_fly)
        //        targetRotation *= Quaternion.Euler(90, 0, 0);

        //    Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, finalTurnSmoothing * Time.deltaTime);
        //    GetComponent<Rigidbody>().MoveRotation(newRotation);
        //    _lastDirection = targetDirection;
        //}
        ////idle - fly or grounded
        //if (!(Mathf.Abs(_h) > 0.9 || Mathf.Abs(_v) > 0.9))
        //{
        //    Repositioning();
        //}
           
        if (targetDirection.sqrMagnitude > 0.1f)
            transform.LookAt(transform.position + targetDirection);

        return targetDirection;
	}	

	private void Repositioning()
	{
		Vector3 repositioning = _movementLastDirection;
		if(repositioning != Vector3.zero)
		{
			repositioning.y = 0;
			Quaternion targetRotation = Quaternion.LookRotation (repositioning, Vector3.up);
			Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, _movementTurnSmoothing * Time.deltaTime);
			GetComponent<Rigidbody>().MoveRotation (newRotation);
		}
	}

    public bool IsFlying()
    {
        return _movementFly;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, _movementDistanceToGround + 0.1f);
    }

    public bool IsSprinting()
    {
        return _inputSprint && !_inputAim && (IsMoving());
    }

    public bool IsMoving()
    {
        return Mathf.Abs(_inputHorizontal) > 0.1 || Mathf.Abs(_inputVertical) > 0.1;
    }

    public new bool IsAiming()
    {
        return _inputAim && !_movementFly;
    }

    #endregion

    public override void SetDestinationPosition(Vector3 destination)
    {
        throw new NotImplementedException();
    }



}
