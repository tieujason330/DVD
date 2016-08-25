using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierGroup : BaseAIUnitGroup
{
    public Captain _captain;
    public Soldier[] _soldiers;

    void Awake()
    {
        _soldiers = GetComponentsInChildren<Soldier>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void PerformGroupAction(string _action)
    {
        foreach (Soldier soldier in _soldiers)
        {
            soldier.PerformAction(_action);
        }
    }
}
