using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoldierGroup : BaseGroup
{
    public Captain _captain;

    void Awake()
    {
        base.Awake();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void ExecuteCommand(BaseCommand command)
    {
        foreach (Soldier soldier in _units)
        {
            soldier.ExecuteCommand(command);
        }
    }
}
