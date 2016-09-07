using UnityEngine;
using System.Collections;
using System;

public class Soldier : BaseAIUnit
{
    public SoldierGroup _soldierGroup;
    //public BaseWorldCharacter _followCharacter;
    private NavMeshAgent _navMeshAgent;

    private int _animationRunParameter;
    private int _animatorAttackParameter;
    private int _animatorDamagedParameter;

    private bool _runBool;
    private bool _attackBool;

    public float _maxAttackDistance = 0.0f;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _currentHealth = _initialHealth;

        _animationRunParameter = Animator.StringToHash("Run");
        _animatorAttackParameter = Animator.StringToHash("Attack");
        _animatorDamagedParameter = Animator.StringToHash("Damaged");
    }

    // Use this for initialization
    void Start ()
    {
        _navMeshAgent = GetComponentInChildren<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    base.Update();

        NavigationLogic();
        AttackLogic();
	    UpdateAnimations();
	}

    void NavigationLogic()
    {
        if (_followDetectionCharacter != null)
        {
            _navMeshAgent.SetDestination(_followDetectionCharacter.gameObject.transform.position);
            if (Vector3.Distance(_followDetectionCharacter.transform.position, transform.position) <=
                _navMeshAgent.stoppingDistance)
            {
                _runBool = false;
            }
            else
                _runBool = true;
        }
        else
            _runBool = false;
    }

    void AttackLogic()
    {
        //for debug
        Vector3 forward = transform.TransformDirection(Vector3.forward)*2;
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

    public override void PerformAction(string action)
    {
        // Do soldier action
        PerformOwnAction(action);
    }

    public override void PerformOwnAction(string action)
    {
        _animator.speed = 1f;
        _animator.Play(action);
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

    public override bool IsAttacking()
    {
        //value of 1 is end of anim
        //value of 0.5 is end of anim
        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(Consts.ANIMATION_ATTACK_LAYER);
        if (animStateInfo.IsName("Attack"))
        {
            return (animStateInfo.normalizedTime < 1.0f || animStateInfo.loop);
        }
        //Added AttackLoopBuffer in Animator to allow return false during loop
        return false;
    }

    public override void GiveDamage(float damage, BaseWorldCharacter attackedCharacter)
    {
        if (attackedCharacter != null)
            attackedCharacter.TakeDamage(damage);
    }

    public override void FollowDetectionCharacter(BaseWorldCharacter character)
    {
        if (_followDetectionCharacter == null || character == null)
            _followDetectionCharacter = character;
    }

    public override void AttackDetectionCharacter(BaseWorldCharacter character)
    {
        if (_attackDetectionCharacter == null || character == null)
            _attackDetectionCharacter = character;
    }

    public void InitializeCharacter()
    {
        //after being set inactive, reactivating
        _currentHealth = _initialHealth;
    }
}
