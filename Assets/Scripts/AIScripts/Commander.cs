using UnityEngine;
using System.Collections;
using System;

public class Commander : BaseAIUnit
{
    public CaptainGroup _captainGroup;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void PerformAction(string _action)
    {
        // Do commander action
        PerformOwnAction(_action);
        _captainGroup.PerformGroupAction(_action);
    }

    public override void PerformOwnAction(string _action)
    {
        _animator.speed = 1f;
        _animator.Play(_action);
    }

    public override void TakeDamage(float damage)
    {
        Debug.Log(string.Format("{0} ouch.", gameObject.name));
    }

    public override bool IsAttacking()
    {
        throw new NotImplementedException();
    }

    public override void GiveDamage(float damage, BaseWorldCharacter attackedCharacter)
    {
        throw new NotImplementedException();
    }

    public override void DetectCharacter(BaseWorldCharacter character)
    {
        throw new NotImplementedException();
    }
}
