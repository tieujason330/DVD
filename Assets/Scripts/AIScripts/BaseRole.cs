using UnityEngine;
using System.Collections;

public abstract class BaseRole : MonoBehaviour {

    public BaseWorldCharacter _myCharacter;

    public void Awake()
    {
        InitializeRole();
    }

    // Use this for initialization
    public void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
	
	}

    public void ExecuteCommand(BaseCommand command)
    {
        // Do command
        if (command._commandType == CommandType.Attack)
        {
            AttackCommand attackCommand = (AttackCommand)command;
            _myCharacter.SetTargetCharacter(attackCommand._targetCharacter);
        }
        else
        {
            //other command types
        }
    }

    public abstract void PerformOwnAction(string action);

    public void InitializeRole()
    {
        _myCharacter = GetComponentInParent<BaseWorldCharacter>();
    }
}
