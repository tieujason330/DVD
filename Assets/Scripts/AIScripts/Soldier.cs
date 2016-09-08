using UnityEngine;
using System.Collections;
using System;

public class Soldier : CharacterRole
{
    public SoldierGroup _soldierGroup;

    void Awake()
    {
        //base.Awake();
    }

    // Use this for initialization
    void Start ()
    {
        //base.Start();
    }
	
	// Update is called once per frame
	void Update ()
	{
	    //base.Update();
	}

    public override void PerformAction(string action)
    {
        // Do soldier action
        PerformOwnAction(action);
    }

    public override void PerformOwnAction(string action)
    {
        //_animator.speed = 1f;
        //_animator.Play(action);
    }

    public override void InitializeRole()
    {
        Debug.Log("Soldier init.");
    }
}
