using UnityEngine;
using System.Collections;
using System;
using System.Linq;

// CommanderGroup creates the orders
public class CommanderGroup : BaseGroup
{
    private string[] _animations;
    private int _currentNumber;

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

    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 60, 40), "Attack"))
        {
            AttackCommand attackCommand = new AttackCommand(GameObject.FindGameObjectWithTag(Consts.TAG_PLAYER).GetComponent<BaseWorldCharacter>());
            ExecuteCommand(attackCommand);
        }
    }

    public override void ExecuteCommand(BaseCommand command)
    {
        foreach (Commander commander in _units)
        {
            commander.ExecuteCommand(command);
        }
    }
}
