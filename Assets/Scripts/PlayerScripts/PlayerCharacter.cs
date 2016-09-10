using System;
using UnityEngine;
using System.Collections;

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

	private Animator _anim;
	private int _speedFloat;
	private int _jumpBool;
	private int _hFloat;
	private int _vFloat;
	private int _aimBool;
	private int _flyBool;
	private int _groundedBool;
    private int _attackAnimationID;
    private int _damagedTrigger;
	private Transform _cameraTransform;

	private float _h;
	private float _v;

	private bool _aim;

	private bool _run;
	private bool _sprint;
    private bool _attackButtonPressed;
	private bool _isMoving;

	// fly
	private bool _fly = false;
	private float _distToGround;
	private float _sprintFactor;

	void Awake()
	{
		_anim = GetComponent<Animator> ();
		_cameraTransform = Camera.main.transform;

		_speedFloat = Animator.StringToHash("Speed");
		_jumpBool = Animator.StringToHash("Jump");
		_hFloat = Animator.StringToHash("H");
		_vFloat = Animator.StringToHash("V");
		_aimBool = Animator.StringToHash("Aim");
        _attackAnimationID = Animator.StringToHash("Attack");
	    _damagedTrigger = Animator.StringToHash("Damaged");
		// fly
		_flyBool = Animator.StringToHash ("Fly");
		_groundedBool = Animator.StringToHash("Grounded");
		_distToGround = GetComponent<Collider>().bounds.extents.y;
		_sprintFactor = _sprintSpeed / _runSpeed;

	    _currentHealth = _initialHealth;

	    _faction = Faction.Player;
	    _status = CharacterStatus.Alive;
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
		_h = Input.GetAxis("Horizontal");
		_v = Input.GetAxis("Vertical");
	    _attackButtonPressed = Input.GetButton("Attack");
		_run = Input.GetButton ("Run");
		_sprint = Input.GetButton ("Sprint");
		_isMoving = Mathf.Abs(_h) > 0.1 || Mathf.Abs(_v) > 0.1;
	}

	void FixedUpdate()
	{
		_anim.SetBool (_aimBool, IsAiming());
		_anim.SetFloat(_hFloat, _h);
		_anim.SetFloat(_vFloat, _v);
		
		// Fly
		_anim.SetBool (_flyBool, _fly);
		GetComponent<Rigidbody>().useGravity = !_fly;
		_anim.SetBool (_groundedBool, IsGrounded ());
		if(_fly)
			FlyManagement(_h,_v);

		else
		{
			MovementManagement (_h, _v, true, _sprint);
			JumpManagement ();
		    AttackManagement();
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
			_anim.SetBool (_jumpBool, false);
			if(_timeToNextJump > 0)
				_timeToNextJump -= Time.deltaTime;
		}
		if (Input.GetButtonDown ("Jump"))
		{
			_anim.SetBool(_jumpBool, true);
			if(_speed > 0 && _timeToNextJump <= 0 && !_aim)
			{
				GetComponent<Rigidbody>().velocity = new Vector3(0, jumpHeight, 0);
				_timeToNextJump = jumpCooldown;
			}
		}
	}

    void AttackManagement()
    {
        _anim.SetBool(_attackAnimationID, _attackButtonPressed);
    }

	void MovementManagement(float horizontal, float vertical, bool running, bool sprinting)
	{
		Rotating(horizontal, vertical);

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

			_anim.SetFloat(_speedFloat, _speed, _speedDampTime, Time.deltaTime);
		}
		else
		{
			_speed = 0f;
			_anim.SetFloat(_speedFloat, 0f);
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

		if(IsAiming())
		{
			targetDirection = forward;
			finalTurnSmoothing = _aimTurnSmoothing;
		}
		else
		{
			targetDirection = forward * vertical + right * horizontal;
			finalTurnSmoothing = _turnSmoothing;
		}

		if((_isMoving && targetDirection != Vector3.zero) || IsAiming())
		{
			Quaternion targetRotation = Quaternion.LookRotation (targetDirection, Vector3.up);
			// fly
			if (_fly)
				targetRotation *= Quaternion.Euler (90, 0, 0);

			Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, finalTurnSmoothing * Time.deltaTime);
			GetComponent<Rigidbody>().MoveRotation (newRotation);
			_lastDirection = targetDirection;
		}
		//idle - fly or grounded
		if(!(Mathf.Abs(_h) > 0.9 || Mathf.Abs(_v) > 0.9))
		{
			Repositioning();
		}

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

	public bool IsAiming()
	{
		return _aim && !_fly;
	}

	public bool IsSprinting()
	{
		return _sprint && !_aim && (_isMoving);
	}

    public override bool IsAttacking()
    {
        //value of 1 is end of anim
        //value of 0.5 is end of anim
        AnimatorStateInfo animStateInfo = _anim.GetCurrentAnimatorStateInfo(Consts.ANIMATION_ATTACK_LAYER);
        if (animStateInfo.IsName("Attack"))
        {
            return (animStateInfo.normalizedTime < 1.0f || animStateInfo.loop) && _attackFrame;
        }
        //Added AttackLoopBuffer in Animator to allow return false during loop
        return false;
    }

    public override void GiveDamage(float damage, BaseWorldCharacter attackedCharacter)
    {
        if (attackedCharacter != null)
            attackedCharacter.TakeDamage(damage);
    }

    public override void TakeDamage(float damage)
    {
        _anim.SetTrigger(_damagedTrigger);
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
