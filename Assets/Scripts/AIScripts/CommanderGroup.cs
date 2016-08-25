using UnityEngine;
using System.Collections;
using System;

// CommanderGroup creates the orders
public class CommanderGroup : BaseAIUnitGroup
{
    public Commander[] _commanders;
    private string[] _animations;
    private int _currentNumber;

    void Awake()
    {
        _commanders = GetComponentsInChildren<Commander>();

        _animations = new string[] {
            "standing_melee_attack_downward",
            "Teat_01",
            "Basic_Run_01",
            "Basic_Run_02",
            "Basic_Run_03",
            "Basic_Walk_01",
            "Basic_Walk_02",
            "Etc_Walk_Zombi_01"

        };

        _currentNumber = 0;
    }

    // Use this for initialization
    void Start () {
        PerformGroupAction(_animations[_currentNumber]);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 50, 50), "<"))
        {
            _currentNumber--;

            if (_currentNumber < 0)
            {
                _currentNumber = _animations.Length - 1;
            }

            PerformGroupAction(_animations[_currentNumber]);

        }

        if (GUI.Button(new Rect(250, 50, 50, 50), ">"))
        {
            _currentNumber++;

            if (_currentNumber == _animations.Length)
            {
                _currentNumber = 0;
            }

            PerformGroupAction(_animations[_currentNumber]);
        }

        GUI.Label(new Rect(125, 50, 80, 80), "Commander Controls");
        GUI.Label(new Rect(125, 80, 200, 100), _animations[_currentNumber].ToString());

    }

    public override void PerformGroupAction(string _action)
    {
        foreach(Commander commander in _commanders)
            commander.PerformAction(_action);
    }
}
