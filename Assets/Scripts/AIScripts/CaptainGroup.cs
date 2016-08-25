using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaptainGroup : BaseAIUnitGroup
{
    public Commander _commander;
    public Captain[] _captains;

    void Awake()
    {
        _captains = GetComponentsInChildren<Captain>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void PerformGroupAction(string _action)
    {
        foreach (Captain captain in _captains)
        {
            captain.PerformAction(_action);
        }
    }
}
