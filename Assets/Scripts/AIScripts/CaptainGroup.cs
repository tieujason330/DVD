using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaptainGroup : BaseGroup
{
    public Commander _commander;

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
        foreach (Captain captain in _units)
        {
            captain.ExecuteCommand(command);
        }
    }
}
