using UnityEngine;
using System.Collections;
using System;

public class Soldier : BaseRole
{
    public SoldierGroup _soldierGroup;

    void Awake()
    {
        base.Awake();
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

    public new void ExecuteCommand(BaseCommand command)
    {
        base.ExecuteCommand(command);
    }

    public override void PerformOwnAction(string action)
    {
        //_animator.speed = 1f;
        //_animator.Play(action);
    }

    public new void InitializeRole()
    {
        base.InitializeRole();
        Debug.Log("Soldier init.");
    }
}
