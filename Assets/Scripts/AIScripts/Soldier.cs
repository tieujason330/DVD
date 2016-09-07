using UnityEngine;
using System.Collections;
using System;

public class Soldier : BaseAIUnit
{
    public SoldierGroup _soldierGroup;

    void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start ()
    {
        base.Start();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    base.Update();
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


    public void InitializeCharacter()
    {
        //after being set inactive, reactivating
        _currentHealth = _initialHealth;
    }
}
