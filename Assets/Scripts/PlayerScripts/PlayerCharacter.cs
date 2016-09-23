using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerCharacter : BaseWorldCharacter
{
    public float _walkSpeed = 0.15f;
	public float _runSpeed = 1.0f;
	public float _sprintSpeed = 2.0f;
	public float _flySpeed = 4.0f;

	public float _turnSmoothing = 3.0f;
	public float _aimTurnSmoothing = 15.0f;
	public float _speedDampTime = 0.1f;

	public float jumpHeight = 5.0f;
	public float jumpCooldown = 1.0f;

	private float _timeToNextJump = 0;

    private float _speed;

	private Vector3 _lastDirection;

	private Animator _animator;
	private int _speedFloat;
	private int _jumpBool;
	private int _hFloat;
	private int _vFloat;
	private int _aimBool;
	private int _flyBool;
	private int _groundedBool;
    private int _meleeEquipped;
    private int _meleeAttackBool;
    private int _meleeAttackComboCounterInt;
    private int _damagedTrigger;
    private int _rollTrigger;
    private int _meleeComboTimedOutBool;
	private Transform _cameraTransform;

	private float _h;
	private float _v;

	private bool _aim;

	private bool _run;
	private bool _sprint;
    private bool _roll;

    public bool _attackButtonPressed;
	private bool _isMoving;

    public int _maxComboCount;
    public int _attackComboCounter;
    public int _attackComboPoints;
    private AnimatorStateInfo _previousAnimationStateAttackClicked;
    public float _initialComboTimer;
    public float _currentComboTimer;
    private ComboTimer _comboTimer;
    private ComboPoints _comboPoints;
    private bool _isAttacking;
    private float _comboTimerDuration = 0.5f;

    public AnimationClip[] _animationClips;

	// fly
	private bool _fly = false;
	private float _distToGround;
	private float _sprintFactor;

	void Awake()
	{
		_animator = GetComponent<Animator> ();
		_cameraTransform = Camera.main.transform;
	    _comboTimer = GetComponentInChildren<ComboTimer>();
        _comboTimer.gameObject.SetActive(false);
	    _comboPoints = GetComponentInChildren<ComboPoints>();

		_speedFloat = Animator.StringToHash("Speed");
		_jumpBool = Animator.StringToHash("Jump");
		_hFloat = Animator.StringToHash("H");
		_vFloat = Animator.StringToHash("V");
		_aimBool = Animator.StringToHash("Aim");
	    _meleeEquipped = Animator.StringToHash("MeleeEquipped");
        _meleeAttackBool = Animator.StringToHash("MeleeAttack");
	    _meleeAttackComboCounterInt = Animator.StringToHash("AttackComboCounter");
	    _damagedTrigger = Animator.StringToHash("Damaged");
	    _meleeComboTimedOutBool = Animator.StringToHash("ComboTimedOut");
	    _rollTrigger = Animator.StringToHash("Roll");

		// fly
		_flyBool = Animator.StringToHash ("Fly");
		_groundedBool = Animator.StringToHash("Grounded");
		_distToGround = GetComponent<Collider>().bounds.extents.y;
		_sprintFactor = _sprintSpeed / _runSpeed;

	    _currentHealth = _initialHealth;

	    _faction = Faction.Player;
	    _status = CharacterStatus.Alive;

	    _currentComboTimer = _initialComboTimer;
	}

    void Start()
    {
        //test
        //gameObject.AddComponent<Soldier>().InitializeRole();
    }

	bool IsGrounded() {
		return Physics.Raycast(transform.position, -Vector3.up, _distToGround + 0.1f);
	}

	void Update()
	{
        base.Update();
		// fly
		if(Input.GetButtonDown ("Fly"))
			_fly = !_fly;
		_aim = Input.GetButton("Aim");
		_h = Input.GetAxis("MoveHorizontal");
		_v = Input.GetAxis("MoveVertical");

	    _attackButtonPressed = Input.GetButtonDown("Attack");
        _run = Input.GetButton ("Run");
		_sprint = Input.GetButton ("Sprint");
		_isMoving = Mathf.Abs(_h) > 0.1 || Mathf.Abs(_v) > 0.1;
	    _roll = Input.GetButtonDown("Roll");

		_animator.SetBool (_aimBool, IsAiming());
		_animator.SetFloat(_hFloat, _h);
		_animator.SetFloat(_vFloat, _v);

        if (_comboTimer.gameObject.activeSelf)
	        ComboTimerTick();

		// Fly
		_animator.SetBool (_flyBool, _fly);
		GetComponent<Rigidbody>().useGravity = !_fly;
		_animator.SetBool (_groundedBool, IsGrounded ());

		if(_fly)
			FlyManagement(_h,_v);

		else
		{
            MovementManagement (_h, _v, true, _sprint);
			JumpManagement ();
		    //OverrideAnimationTest();
            if (_roll)
                _animator.SetTrigger(_rollTrigger);

		}
	}

	// fly
	void FlyManagement(float horizontal, float vertical)
	{
		Vector3 direction = Rotating(horizontal, vertical);
		GetComponent<Rigidbody>().AddForce(direction * _flySpeed * 100 * (_sprint?_sprintFactor:1));
	}

	void JumpManagement()
	{
		if (GetComponent<Rigidbody>().velocity.y < 10) // already jumped
		{
			_animator.SetBool (_jumpBool, false);
			if(_timeToNextJump > 0)
				_timeToNextJump -= Time.deltaTime;
		}
		if (Input.GetButtonDown ("Jump"))
		{
			_animator.SetBool(_jumpBool, true);
			if(_speed > 0 && _timeToNextJump <= 0 && !_aim)
			{
				GetComponent<Rigidbody>().velocity = new Vector3(0, jumpHeight, 0);
				_timeToNextJump = jumpCooldown;
			}
		}
	}

    #region Attack logic

    void ComboTimerTick()
    {
        _currentComboTimer -= Time.deltaTime;
        if (_currentComboTimer < 0.0f)
        {
            DisableComboTimer();
        }
    }

    void ResetComboTimer()
    {
        if (!_comboTimer.gameObject.activeSelf)
        {
            _currentComboTimer = _initialComboTimer = _comboTimerDuration;
            _comboTimer.gameObject.SetActive(true);
            _animator.SetBool(_meleeComboTimedOutBool, false);
        }
    }

    void DisableComboTimer()
    {
        _currentComboTimer = 0.0f;
        _comboTimer.gameObject.SetActive(false);
        _animator.SetBool(_meleeComboTimedOutBool, true);
    }

    private int count = 0;
    void OverrideAnimationTest()
    {
        if (count > 0) return;
        count++;
        RuntimeAnimatorController runtime = _animator.runtimeAnimatorController;
        AnimatorOverrideController over = new AnimatorOverrideController();
        over.runtimeAnimatorController = runtime;

        string animName = "MeleeAttack0";

        for (int i = 0; i < _animationClips.Length; i++)
        {
            over[animName + (i + 1)] = _animationClips[i];
        }


        _animator.runtimeAnimatorController = over;
    }

    public void CancelMeleeCombo()
    {
        DisableComboTimer();
        _attackComboCounter = 0;
        _attackComboPoints = 0;
        UpdateAttackFields();
    }

    public void MeleeInitializeBufferTime()
    {
        ResetComboTimer();
    }

    public void MeleeNotPressedInState(MeleeState state = MeleeState.UndeterminedState)
    {
        //if (state == MeleeState.BufferState)
        //{
        //    ResetComboTimer(animStateInfo.length);
        //}
        if (state == MeleeState.IdleState)
        {
            _attackComboCounter = 0;
            _attackComboPoints = 0;
        }

        UpdateAttackFields();
    }

    public void MeleePressedInState(MeleeState state)
    {
        if (state == MeleeState.AttackState)
        {
            _attackComboPoints = 0;
        }
        else if (state == MeleeState.BufferState)
        {
            if (_attackComboCounter >= _maxComboCount)
                _attackComboCounter = 1;
            else
                _attackComboCounter++;

            if (_attackComboPoints < _maxComboCount)
                _attackComboPoints++;

            DisableComboTimer();
        }

        UpdateAttackFields();
    }

    public void UpdateAttackFields()
    {
        _comboPoints.SetComboPoints(_attackComboPoints, _maxComboCount);
        
        _animator.SetInteger(_meleeAttackComboCounterInt, _attackComboCounter);
        _animator.SetBool(_meleeAttackBool, _attackButtonPressed);
    }

    #endregion

    void MovementManagement(float horizontal, float vertical, bool running, bool sprinting)
    {
        if (IsAttacking()) return;

		Rotating(-vertical, -horizontal);

		if(_isMoving)
		{
			if(sprinting)
			{
				_speed = _sprintSpeed;
			}
			else if (running)
			{
				_speed = _runSpeed;
			}
			else
			{
				_speed = _walkSpeed;
			}

			_animator.SetFloat(_speedFloat, _speed, _speedDampTime, Time.deltaTime);
		}
		else
		{
			_speed = 0f;
			_animator.SetFloat(_speedFloat, 0f);
		}
		GetComponent<Rigidbody>().AddForce(Vector3.forward*_speed);
	}
	Vector3 Rotating(float horizontal, float vertical)
	{
        Vector3 forward = _cameraTransform.TransformDirection(Vector3.forward);
        if (!_fly)
            forward.y = 0.0f;
        forward = forward.normalized;

        Vector3 right = new Vector3(forward.z, 0, -forward.x);

        Vector3 targetDirection;

        float finalTurnSmoothing;

        if (IsAiming())
        {
            targetDirection = forward;
            finalTurnSmoothing = _aimTurnSmoothing;
        }
        else
        {
            targetDirection = forward * vertical + right * horizontal;
            finalTurnSmoothing = _turnSmoothing;
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
		Vector3 repositioning = _lastDirection;
		if(repositioning != Vector3.zero)
		{
			repositioning.y = 0;
			Quaternion targetRotation = Quaternion.LookRotation (repositioning, Vector3.up);
			Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, _turnSmoothing * Time.deltaTime);
			GetComponent<Rigidbody>().MoveRotation (newRotation);
		}
	}

	public bool IsFlying()
	{
		return _fly;
	}

	public override bool IsAiming()
	{
		return _aim && !_fly;
	}

	public bool IsSprinting()
	{
		return _sprint && !_aim && (_isMoving);
	}

    public void SetIsAttacking(bool attacking)
    {
        _isAttacking = attacking;
    }

    public override bool IsAttacking()
    {
        return _isAttacking;
        ////value of 1 is end of anim
        ////value of 0.5 is end of anim
        //AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(Consts.ANIMATION_ATTACK_LAYER);
        //if (animStateInfo.IsName("MeleeAttack" + _attackComboCounter))
        //{
        //    return (animStateInfo.normalizedTime < 1.0f || animStateInfo.loop) && _attackFrame;
        //}
        ////Added AttackLoopBuffer in Animator to allow return false during loop
        //return false;
    }

    public override void GiveDamage(float damage, BaseWorldCharacter attackedCharacter)
    {
        if (attackedCharacter != null)
            attackedCharacter.TakeDamage(damage);
    }

    public override void TakeDamage(float damage)
    {
        _animator.SetTrigger(_damagedTrigger);
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

    public override void SetDestinationPosition(Vector3 destination)
    {
        throw new NotImplementedException();
    }
}
