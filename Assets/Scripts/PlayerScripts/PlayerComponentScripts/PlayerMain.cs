using UnityEngine;
using System.Collections;
using System;

public class PlayerMain : BaseWorldCharacter
{

    private PlayerCombat _playerCombat;
    private PlayerMovement _playerMovement;
    private PlayerInventory _playerInventory;

    //public float _currentHealth;
    //public float _initialHealth;
    //public Faction _faction;
    //public CharacterStatus _status;
    //public CharacterAction _action;

    private float _inputHorizontal;
    private float _inputVertical;
    private bool _inputAim;
    private bool _inputRun;
    private bool _inputSprint;
    private bool _inputRoll;
    private bool _inputAttack;
    private bool _inputFly;
    //private bool _inputSelectActiveAbility;
    private bool _inputActiveAbility01;

    //Getters
    public float InputHorizontal { get { return _inputHorizontal; } }
    public float InputVertical { get { return _inputVertical; } }
    public bool InputAim { get { return _inputAim; } }
    public bool InputRun { get { return _inputRun; } }
    public bool InputSprint { get { return _inputSprint; } }
    public bool InputRoll { get { return _inputRoll; } }
    public bool InputAttack { get { return _inputAttack; } }
    public bool InputFly { get { return _inputFly; } }
    //public bool InputSelectActiveAbility { get { return _inputSelectActiveAbility; } }
    public bool InputActiveAbility01 { get { return _inputActiveAbility01; } }

    void Awake()
    {
        _playerCombat = gameObject.GetComponent<PlayerCombat>();
        _playerMovement = gameObject.GetComponent<PlayerMovement>();
        _playerInventory = gameObject.GetComponent<PlayerInventory>();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Update_InputLogic();

	    Update_PlayerComponents();
	}

    void Update_InputLogic()
    {
        _inputFly = Input.GetButtonDown("Fly");
        _inputAim = Input.GetButton("Aim");
        _inputHorizontal = Input.GetAxis("MoveHorizontal");
        _inputVertical = Input.GetAxis("MoveVertical");
        _inputAttack = Input.GetButtonDown("Attack");
        _inputRun = Input.GetButton("Run");
        _inputSprint = Input.GetButton("Sprint");
        _inputRoll = Input.GetButtonDown("Roll");
        //_inputSelectActiveAbility = Input.GetButton("SelectActiveAbility");
        _inputActiveAbility01 = Input.GetButtonDown("ActiveAbility01");
    }

    void Update_PlayerComponents()
    {
        _playerMovement.PlayerUpdate();
        _playerCombat.PlayerUpdate();
        _playerInventory.PlayerUpdate();
    }

    public override void GiveDamage(float damage, BaseWorldCharacter attackedCharacter)
    {
        _playerCombat.GiveDamage(damage, attackedCharacter);
    }

    public override void TakeDamage(float damage)
    {
        _playerCombat.TakeDamage(damage);
    }

    public override void SetDestinationPosition(Vector3 destination)
    {
        throw new NotImplementedException();
    }

    public override void OverrideAttackAnimations(AnimationClip[] overridedAnimations)
    {
        throw new NotImplementedException();
    }

    public override void SetupEquipmentLogic(int maxComboCount, AnimationClip[] overridedAnimations)
    {
        throw new NotImplementedException();
    }
}
