using UnityEngine;
using System.Collections;
using System;

public class Captain : BaseRole
{
    public CaptainGroup _captainGroup;
    public SoldierGroup _soldierGroup;

    void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start()
    {
        //base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        //base.Update();
    }

    public new void ExecuteCommand(BaseCommand command)
    {
        base.ExecuteCommand(command);
        // Tell Soldier Group to do command too
        if (_soldierGroup != null)
            _soldierGroup.ExecuteCommand(command);
    }

    public override void PerformOwnAction(string _action)
    {
        //_animator.speed = 1f;
        //_animator.Play(_action);
    }

    public new void InitializeRole()
    {
        base.InitializeRole();
        Debug.Log("Captain init.");
    }
}
