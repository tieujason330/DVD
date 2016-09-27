using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class BaseAIUnit : BaseWorldCharacter
{
    // Unit info
    public float _minAttackDamage = 0.0f;
    public float _maxAttackDamage = 0.0f;

    public NavMeshAgent _navMeshAgent;

    private int _animationRunParameter;
    private int _animatorAttackParameter;
    private int _animatorDamagedParameter;
    private int _animatorArmedParameter;
    private int _animatorUnarmedParameter;

    private bool _runBool;
    private bool _attackBool;

    protected Animator _animator;

    private MeleeWeapon _meleeWeapon;
    private bool _weaponArmed;

    public void Awake()
    {
        _status = CharacterStatus.Alive;

        _animator = GetComponent<Animator>();
        _currentHealth = _initialHealth;

        _meleeWeapon = GetComponentInChildren<MeleeWeapon>();
        _meleeWeapon.gameObject.SetActive(false);

        _animationRunParameter = Animator.StringToHash("Run");
        _animatorAttackParameter = Animator.StringToHash("Attack");
        _animatorDamagedParameter = Animator.StringToHash("Damaged");
        _animatorArmedParameter = Animator.StringToHash("Armed");
        _animatorUnarmedParameter = Animator.StringToHash("Unarmed");
    }

    // Use this for initialization
    public void Start ()
    {
        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }
	
	// Update is called once per frame
	public void Update ()
	{
        base.Update();

	    if (_currentHealth <= 0)
	    {
	        gameObject.SetActive(false);
	        return;
	    }

	    if (_followDetectionCharacter != null && _followDetectionCharacter._status == CharacterStatus.Dead)
	        _followDetectionCharacter = null;
	    if (_attackDetectionCharacter != null && _attackDetectionCharacter._status == CharacterStatus.Dead)
	        _attackDetectionCharacter = null;

        NavigationLogic();
        AttackLogic();
        UpdateAnimations();
    }

    void NavigationLogic()
    {
        if (_action == CharacterAction.MoveTo)
        {
            _navMeshAgent.stoppingDistance = 0.1f;
            _navMeshAgent.SetDestination(_destinationPosition);
            _runBool = !(Vector3.Distance(_destinationPosition, transform.position) <=
             _navMeshAgent.stoppingDistance);
            if (!_runBool) _action = CharacterAction.None;
            return;
        }

        if (_followDetectionCharacter != null)
        {
            _navMeshAgent.stoppingDistance = 1.7f;
            _navMeshAgent.SetDestination(_followDetectionCharacter.gameObject.transform.position);
            _runBool = !(Vector3.Distance(_followDetectionCharacter.transform.position, transform.position) <=
                         _navMeshAgent.stoppingDistance);
            
        }
        else
        {
            _navMeshAgent.SetDestination(_navMeshAgent.transform.position);
            _runBool = false;
        }
    }

    public override void SetDestinationPosition(Vector3 destination)
    {
        _destinationPosition = destination;
        _action = CharacterAction.MoveTo;
    }

    void AttackLogic()
    {
        //for debug
        Vector3 forward = transform.TransformDirection(Vector3.forward) * 2;
        Debug.DrawRay(transform.position, forward, Color.green);
        if (_attackDetectionCharacter)
        {
            _attackBool = true;
            Vector3 lookDirection = _attackDetectionCharacter.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
        else
        {
            _attackBool = false;
        }
        //Note - don't want to do raycast b/c raycast can't detect gameobject w/ NavMeshAgent on it WTF..
        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, forward, out hit, _maxAttackDistance))
        //{
        //    if (hit.collider.gameObject.GetComponent<BaseWorldCharacter>()._faction != this._faction)
        //        _attackBool = true;
        //}
        //else
        //    _attackBool = false;
    }

    void UpdateAnimations()
    {
        _animator.SetBool(_animationRunParameter, _runBool);
        _animator.SetBool(_animatorAttackParameter, _attackBool);
    }

    public override void TakeDamage(float damage)
    {
        _animator.SetTrigger(_animatorDamagedParameter);
        if (_currentHealth > damage)
            _currentHealth -= damage;
        else
        {
            _currentHealth = 0;
        }
        Debug.Log(string.Format("{0} ==> {1}HP", gameObject.name, _currentHealth));
    }

    //public virtual bool IsAttacking()
    //{
    //    //value of 1 is end of anim
    //    //value of 0.5 is end of anim
    //    AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(Consts.ANIMATION_ATTACK_LAYER);
    //    if (animStateInfo.IsName("Attack"))
    //    {
    //        return (animStateInfo.normalizedTime < 1.0f || animStateInfo.loop) && _attackFrame;
    //    }
    //    //Added AttackLoopBuffer in Animator to allow return false during loop
    //    return false;
    //}

    public override void GiveDamage(float damage, BaseWorldCharacter attackedCharacter)
    {
        if (attackedCharacter != null)
            attackedCharacter.TakeDamage(damage);
    }

    public void FollowDetectionCharacter(BaseWorldCharacter character)
    {
        if (_followDetectionCharacter == null || character == null)
        {
            _followDetectionCharacter = character;
            _action = CharacterAction.Follow;

            if (character == null)
            {
                _animator.SetTrigger(_animatorUnarmedParameter);
                _weaponArmed = false;
            }
            else
            {
                _animator.SetTrigger(_animatorArmedParameter);
                _weaponArmed = true;
            }
        }
    }

    public void SetWeaponActive()
    {
        _meleeWeapon.gameObject.SetActive(_weaponArmed);
    }

    public void AttackDetectionCharacter(BaseWorldCharacter character)
    {
        if (_attackDetectionCharacter == null || character == null)
        {
            _attackDetectionCharacter = character;
            _action = CharacterAction.Attack;
        }
    }

    public new bool IsAiming()
    {
        throw new NotImplementedException();
    }

    public override void OverrideAttackAnimations(AnimationClip[] overridedAnimations)
    {
        throw new NotImplementedException();
    }

    public override void SetupEquipmentLogic(int maxComboCount, AnimationClip[] overridedAnimations)
    {
        throw new NotImplementedException();
    }
}
