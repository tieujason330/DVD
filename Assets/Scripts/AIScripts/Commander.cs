using UnityEngine;
using System.Collections;
using System;

public class Commander : CharacterRole
{
    public CaptainGroup _captainGroup;

    void Awake()
    {
        //base.Awake();
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

    public override void PerformAction(string _action)
    {
        // Do commander action
        PerformOwnAction(_action);
        _captainGroup.PerformGroupAction(_action);
    }

    public override void PerformOwnAction(string _action)
    {
        //_animator.speed = 1f;
        //_animator.Play(_action);
    }

    public override void InitializeRole()
    {
        throw new NotImplementedException();
    }
}
