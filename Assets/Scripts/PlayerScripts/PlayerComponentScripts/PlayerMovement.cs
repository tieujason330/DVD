using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public PlayerMain _playerMain;
    public PlayerCombat _playerCombat;

    private Transform _cameraTransform;

    private Animator _animator;
    private int _animatorSpeedParameter;
    private int _animatorJumpParameter;
    private int _animatorHorizontalParameter;
    private int _animatorVerticalParameter;
    private int _animatorAimParameter;
    private int _animatorFlyParameter;
    private int _animtorGroundedParameter;

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

    void Awake()
    {
        InitializePlayerLogic();
        InitializeAnimatorLogic();
    }

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    #region Initialization

    void InitializePlayerLogic()
    {
        _playerMain = gameObject.GetComponent<PlayerMain>();
        _playerCombat = gameObject.GetComponent<PlayerCombat>();
        _cameraTransform = Camera.main.transform;
    }

    void InitializeAnimatorLogic()
    {
        _animator = GetComponent<Animator>();

        _animatorSpeedParameter = Animator.StringToHash("Speed");
        _animatorJumpParameter = Animator.StringToHash("Jump");
        _animatorHorizontalParameter = Animator.StringToHash("H");
        _animatorVerticalParameter = Animator.StringToHash("V");
        _animatorAimParameter = Animator.StringToHash("Aim");
        _animatorFlyParameter = Animator.StringToHash("Fly");
        _animtorGroundedParameter = Animator.StringToHash("Grounded");
    }

    #endregion

    public void PlayerUpdate()
    {
        if (!_animator.isInitialized) return;

        _animator.SetBool(_animatorAimParameter, IsAiming());
        _animator.SetFloat(_animatorHorizontalParameter, _playerMain.InputHorizontal);
        _animator.SetFloat(_animatorVerticalParameter, _playerMain.InputVertical);
        _animator.SetBool(_animatorFlyParameter, _movementFly);
        GetComponent<Rigidbody>().useGravity = !_movementFly;
        _animator.SetBool(_animtorGroundedParameter, IsGrounded());

        if (_playerMain.InputFly)
            _movementFly = !_movementFly;

        if (_movementFly)
            FlyManagement(_playerMain.InputHorizontal, _playerMain.InputVertical);
        else
        {
            MovementManagement(_playerMain.InputHorizontal, _playerMain.InputVertical, true, _playerMain.InputSprint);
            JumpManagement();
        }
    }

    // fly
    void FlyManagement(float horizontal, float vertical)
    {
        Vector3 direction = Rotating(horizontal, vertical);
        GetComponent<Rigidbody>().AddForce(direction * _movementFlySpeed * 100 * (_playerMain.InputSprint ? _movementSprintFactor : 1));
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
            if (_movementSpeed > 0 && _movementTimeToNextJump <= 0 && !_playerMain.InputAim)
            {
                GetComponent<Rigidbody>().velocity = new Vector3(0, _movementJumpHeight, 0);
                _movementTimeToNextJump = _movementJumpCooldown;
            }
        }
    }

    void MovementManagement(float horizontal, float vertical, bool running, bool sprinting)
    {
        if (_playerMain.IsAttacking || _playerMain.IsUsingAbility) return;

        Rotating(-vertical, -horizontal);

        if (IsMoving())
        {
            if (sprinting)
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
        GetComponent<Rigidbody>().AddForce(Vector3.forward * _movementSpeed);
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
        if (repositioning != Vector3.zero)
        {
            repositioning.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(repositioning, Vector3.up);
            Quaternion newRotation = Quaternion.Slerp(GetComponent<Rigidbody>().rotation, targetRotation, _movementTurnSmoothing * Time.deltaTime);
            GetComponent<Rigidbody>().MoveRotation(newRotation);
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
        return _playerMain.InputSprint && !_playerMain.InputAim && (IsMoving());
    }

    public bool IsMoving()
    {
        return Mathf.Abs(_playerMain.InputHorizontal) > 0.1 || Mathf.Abs(_playerMain.InputVertical) > 0.1;
    }

    public bool IsAiming()
    {
        return _playerMain.InputAim && !_movementFly;
    }

}
