using UnityEngine;
using System.Collections;
using System;

public class Soldier : BaseAIUnit
{
    private const int ANIMATION_ATTACK_LAYER = 1;

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
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update ()
	{
        NavigationLogic();
	    UpdateAnimations();
	}

    void NavigationLogic()
    {
        if (_followCharacter != null)
        {
            _runBool = true;
            _navMeshAgent.SetDestination(_followCharacter.gameObject.transform.position);
            AttackLogic();
        }
        else
            _runBool = false;
    }

    void AttackLogic()
    {
        //for debug
        Vector3 forward = transform.TransformDirection(Vector3.forward)*2;
        Debug.DrawRay(transform.position, forward, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, forward, out hit, _maxAttackDistance))
        {
            //need to generalize...
            if (hit.collider.gameObject.tag == "Player")
                _attackBool = true;
        }
        else
            _attackBool = false;
    }

    void UpdateAnimations()
    {
        _animator.SetBool(_animationRunParameter, _runBool);
        _animator.SetBool(_animatorAttackParameter, _attackBool);
    }

    public override void PerformAction(string _action)
    {
        // Do soldier action
        PerformOwnAction(_action);
    }

    public override void PerformOwnAction(string _action)
    {
        _animator.speed = 1f;
        _animator.Play(_action);
    }

    public override void TakeDamage(float damage)
    {
        _animator.SetTrigger(_animatorDamagedParameter);
        if (_currentHealth > damage)
            _currentHealth -= damage;
        else
        {
            _currentHealth = 0;
            Destroy(this.gameObject);
        }
        Debug.Log(string.Format("{0} ==> {1}HP", gameObject.name, _currentHealth));
    }

    public override bool IsAttacking()
    {
        //value of 1 is end of anim
        //value of 0.5 is end of anim
        AnimatorStateInfo animStateInfo = _animator.GetCurrentAnimatorStateInfo(ANIMATION_ATTACK_LAYER);
        if (animStateInfo.IsName("Attack"))
        {
            return (animStateInfo.normalizedTime < 1.0f || animStateInfo.loop);
        }
        //Added AttackLoopBuffer in Animator to allow return false during loop
        return false;
    }

    public override void GiveDamage(float damage, BaseWorldCharacter attackedCharacter)
    {
        attackedCharacter.TakeDamage(damage);
    }

    public override void DetectCharacter(BaseWorldCharacter character)
    {
        if (_followCharacter == null || character == null)
            _followCharacter = character;
    }
}
