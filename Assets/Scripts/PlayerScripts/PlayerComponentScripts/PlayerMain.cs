using UnityEngine;
using System.Collections;
using System;

public class PlayerMain : BaseWorldCharacter
{

    private PlayerCombat _playerCombat;
    private PlayerMovement _playerMovement;
    private PlayerInventory _playerInventory;
    private PlayerEquipment _playerEquipment;

    //public float _currentHealth;
    //public float _initialHealth;
    //public Faction _faction;
    //public CharacterStatus _status;
    //public CharacterAction _action;
    public float _currentStamina;
    public float _initialStamina;
    public float _potentialStaminaRegain = 0.0f;

    public const int ATTACK_MAXIMUM_COMBO_COUNT = 6;
    public int _attackMaxComboCountRightArm;
    public float _attackRightArmCost;
    public float _attackRightArmPunishMultiplier;
    public float _attackRightArmCombatPointsGainMultiplier;

    public int _attackMaxComboCountLeftArm;
    public float _attackLeftArmCost;
    public float _attackLeftArmPunishMultiplier;
    public float _attackLeftArmCombatPointsGainMultiplier;

    public float _currentCombatPoints;
    public float _initialCombatPoints = 100.0f;
    //public bool IsAttacking { get; set; }

    private float _inputHorizontal;
    private float _inputVertical;
    private bool _inputAim;
    private bool _inputRun;
    private bool _inputSprint;
    private bool _inputRoll;
    private bool _inputFly;
    //private bool _inputSelectActiveAbility;

    private bool _inputHeadActiveAbility;
    private bool _inputTorsoActiveAbility;
    private bool _inputRightArmActiveAbility;
    private bool _inputLeftArmActiveAbility;
    private bool _inputLegsActiveAbility;
    private bool _inputRightArm;
    public bool _inputLeftArm;

    private bool _inputActiveAbility01;
    private bool _inventoryOpen;
    private bool _paused;


    public ActiveAbility _headActiveAbility;
    public ActiveAbility _torsoActiveAbility;
    public ActiveAbility _rightArmActiveAbility;
    public ActiveAbility _leftArmActiveAbility;
    public ActiveAbility _legsActiveAbility;

    //Getters
    public float InputHorizontal { get { return _inputHorizontal; } }
    public float InputVertical { get { return _inputVertical; } }
    public bool InputAim { get { return _inputAim; } }
    public bool InputRun { get { return _inputRun; } }
    public bool InputSprint { get { return _inputSprint; } }
    public bool InputRoll { get { return _inputRoll; } }
    public bool InputRightArm { get { return _inputRightArm; } }
    public bool InputLeftArm { get { return _inputLeftArm; } }
    public bool InputFly { get { return _inputFly; } }
    //public bool InputSelectActiveAbility { get { return _inputSelectActiveAbility; } }
    public bool InputHeadActiveAbility { get { return _inputHeadActiveAbility; } }
    public bool InputTorsoActiveAbility { get { return _inputTorsoActiveAbility; } }
    public bool InputRightArmActiveAbility { get { return _inputRightArmActiveAbility; } }
    public bool InputLeftArmActiveAbility { get { return _inputLeftArmActiveAbility; } }
    public bool InputLegsActiveAbility { get { return _inputLegsActiveAbility; } }
    public bool IsUsingAbility { get; set; }

    void Awake()
    {
        base.Awake();
        _playerCombat = gameObject.GetComponent<PlayerCombat>();
        _playerMovement = gameObject.GetComponent<PlayerMovement>();
        _playerInventory = gameObject.GetComponent<PlayerInventory>();
        _playerEquipment = gameObject.GetComponent<PlayerEquipment>();
        _attackMaxComboCountRightArm = _initial_attackMaxComboCount;
        _attackMaxComboCountLeftArm = _initial_attackMaxComboCount;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
        Update_InputLogic();
	    Update_PlayerComponents();
	}

    void Update_InputLogic()
    {
        if (!_inventoryOpen)
        {
            _inputFly = Input.GetButtonDown("Fly");
            _inputAim = Input.GetButton("Aim");
            _inputHorizontal = Input.GetAxis("MoveHorizontal");
            _inputVertical = Input.GetAxis("MoveVertical");
            _inputRun = Input.GetButton("Run");
            _inputSprint = Input.GetButton("Sprint");
            _inputRoll = Input.GetButtonDown("Roll");
            //_inputSelectActiveAbility = Input.GetButton("SelectActiveAbility");
            _inputHeadActiveAbility = Input.GetButtonDown("HeadActiveAbility");
            _inputTorsoActiveAbility = Input.GetButtonDown("TorsoActiveAbility");
            _inputLegsActiveAbility = Input.GetButtonDown("LegsActiveAbility");
            _inputRightArm = Input.GetButtonDown("RightArm");
            _inputLeftArm = Input.GetButtonDown("LeftArm");

            SetTriggerInputs();
        }

        if (Input.GetButtonDown("Inventory"))
        {
            _inventoryOpen = GetComponent<PlayerInventory>().Toggle();
            Camera.main.GetComponent<ThirdPersonOrbitCam>().InventoryOpen = _inventoryOpen;
            _inputVertical = 0.0f;
            _inputHorizontal = 0.0f;
        }
    }

    private bool _usingRightTrigger = false;
    private bool _usingLeftTrigger = false;
    void SetTriggerInputs()
    {
        _inputRightArmActiveAbility = _inputLeftArmActiveAbility = false;

        if (!Input.GetAxisRaw("RightArmActiveAbility").Equals(0.0f))
        {
            if (!_usingRightTrigger)
            {
                _usingRightTrigger = true;
                _inputRightArmActiveAbility = true;
            }
        }
        else
        {
            _usingRightTrigger = false;
        }

        if (!Input.GetAxisRaw("LeftArmActiveAbility").Equals(0.0f))
        {
            if (!_usingLeftTrigger)
            {
                _usingLeftTrigger = true;
                _inputLeftArmActiveAbility = true;
            }
        }
        else
        {
            _usingLeftTrigger = false;
        }        
    }

    void Update_PlayerComponents()
    {
        if (_inventoryOpen)
            _playerInventory.PlayerUpdate();
        else
        {
            _playerMovement.PlayerUpdate();
            _playerCombat.PlayerUpdate();
        }
    }

    public override bool GiveDamage(float damage, BaseWorldCharacter attackedCharacter)
    {
        return _playerCombat.GiveDamage(damage, attackedCharacter);
    }

    public override bool TakeDamage(float damage)
    {
        return _playerCombat.TakeDamage(damage);
    }

    public override void SetDestinationPosition(Vector3 destination)
    {
        throw new NotImplementedException();
    }

    public override void SetupEquipmentLogic(BaseEquipment equipment, bool equip)
    {
        _playerEquipment.SetupEquipmentLogic(equipment, equip);
    }
}
