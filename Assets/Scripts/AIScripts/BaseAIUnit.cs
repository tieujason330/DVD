using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public abstract class BaseAIUnit : BaseWorldCharacter
{
    // Unit info
    public float _minAttackDamage = 0.0f;
    public float _maxAttackDamage = 0.0f;

    public BaseWorldCharacter _followDetectionCharacter = null;
    public BaseWorldCharacter _attackDetectionCharacter = null;

    protected Animator _animator;

    void Awake()
    {
        _status = CharacterStatus.Alive;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	public void Update ()
	{
        base.Update();

        if (_currentHealth <= 0)
            gameObject.SetActive(false);

	    if (_followDetectionCharacter != null && _followDetectionCharacter._status == CharacterStatus.Dead)
	        _followDetectionCharacter = null;
	    if (_attackDetectionCharacter != null && _attackDetectionCharacter._status == CharacterStatus.Dead)
	        _attackDetectionCharacter = null;
	}

    public abstract void PerformAction(string _action);
    public abstract void PerformOwnAction(string _action);
    public abstract void FollowDetectionCharacter(BaseWorldCharacter character);
    public abstract void AttackDetectionCharacter(BaseWorldCharacter character);
}
