using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEquipment : MonoBehaviour {

    private Animator _animator;
    private RuntimeAnimatorController _initialRuntimeAnimatorController;
    private AnimatorOverrideController _overrideController;
    public PlayerMain _playerMain;

    public BaseEquipment _headEquipment;
    public BaseEquipment _torsoEquipment;
    public BaseEquipment _rightArmEquipment;
    public BaseEquipment _leftArmEquipment;
    public BaseEquipment _legsEquipment;

    private Transform _headTransform;
    private Transform _torsoTransform;
    private Transform _rightArmTransform;
    private Transform _leftArmTransform;
    private Transform _legsTransform;


    #region Properties
    public Transform HeadTransform
    {
        get { return _headTransform; }
    }

    public Transform TorsoTransform
    {
        get { return _torsoTransform; }
    }

    public Transform RightArmTransfrom
    {
        get { return _rightArmTransform; }
    }

    public Transform LeftArmTransform
    {
        get { return _leftArmTransform; }
    }

    public BaseEquipment HeadEquipment
    {
        get { return _headEquipment; }
        set { _headEquipment = value; }
    }
    public BaseEquipment TorsoEquipment
    {
        get { return _torsoEquipment; }
        set { _torsoEquipment = value; }
    }
    public BaseEquipment RightArmEquipment
    {
        get { return _rightArmEquipment; }
        set { _rightArmEquipment = value; }
    }
    public BaseEquipment LeftArmEquipment
    {
        get { return _leftArmEquipment; }
        set { _leftArmEquipment = value; }
    }
    public BaseEquipment LegsEquipment
    {
        get { return _legsEquipment; }
        set { _legsEquipment = value; }
    }

    #endregion

    void Awake()
    {
        _playerMain = GetComponent<PlayerMain>();

        _animator = GetComponent<Animator>();
        //save original runtime to prevent nesting when overriding (Unity issue)
        _initialRuntimeAnimatorController = _animator.runtimeAnimatorController;
        _overrideController = new AnimatorOverrideController();
        _overrideController.runtimeAnimatorController = _initialRuntimeAnimatorController;


        if (_headEquipment != null)
            _headEquipment.Unequip();

        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "PlayerHead")
            {
                _headTransform = child;
            }
        }
    }
    
    // Use this for initialization
    void Start ()
    {
        InitializeEquipment();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayerUpdate()
    {
        
    }

    public void InitializeEquipment()
    {
        if (_headEquipment != null)
            _headEquipment.gameObject.SetActive(true);
        if (_torsoEquipment != null)
            _torsoEquipment.gameObject.SetActive(true);
        if (_rightArmEquipment != null)
            _rightArmEquipment.gameObject.SetActive(true);
        if (_leftArmEquipment != null)
            _leftArmEquipment.gameObject.SetActive(true);
        if (_legsEquipment != null)
            _legsEquipment.gameObject.SetActive(true);
    }

    public void SetupEquipmentLogic(BaseEquipment equipment, bool equip)
    {
        switch (equipment._EquipmentType)
        {
            case EquipmentType.Helmet:
                SetupHead();
                break;
            case EquipmentType.Armor:
                SetupTorso(equipment as BaseArmor, equip);
                break;
            case EquipmentType.MeleeWeapon:
                SetupMeleeWeapon(equipment as MeleeWeapon, equip);
                break;
        }
    }

    #region Head setup

    private void SetupHead()
    {
        
    }

    #endregion

    #region Armor setup

    private void SetupTorso(BaseArmor armor, bool equip)
    {
        _playerMain._torsoActiveAbility = armor._activeAbility;
        SetupActiveAbility(armor._activeAbility, armor._equipmentComponent);
    }

    #endregion

    #region Left/Right arm setup

    void SetupMeleeWeapon(MeleeWeapon meleeWeapon, bool equip)
    {
        if (equip)
        {
            //set up weapon
            if (meleeWeapon._equipmentComponent == EquipmentComponent.LeftArm)
            {
                _playerMain._attackMaxComboCountLeftArm = meleeWeapon._maxAttackComboCount;
                _playerMain._leftArmActiveAbility = meleeWeapon._activeAbility;
            }
            else if (meleeWeapon._equipmentComponent == EquipmentComponent.RightArm)
            {
                _playerMain._attackMaxComboCountRightArm = meleeWeapon._maxAttackComboCount;
                _playerMain._rightArmActiveAbility = meleeWeapon._activeAbility;
            }

            OverrideEquipmentAnimations(meleeWeapon._attackAnimations, meleeWeapon._equipmentComponent);

            //set up weapon's active ability
            SetupActiveAbility(meleeWeapon._activeAbility, meleeWeapon._equipmentComponent);
        }
        else
        {
            _playerMain._attackMaxComboCountRightArm = _playerMain._initial_attackMaxComboCount;
            OverrideEquipmentAnimations(_playerMain._initial_attackAnimations, meleeWeapon._equipmentComponent);

            SetupActiveAbility(null, meleeWeapon._equipmentComponent);
        }
    }

    public void OverrideEquipmentAnimations(AnimationClip[] overridedAnimations, EquipmentComponent equipmentComponent)
    {
        if (overridedAnimations.Length == 0) return;

        //RuntimeAnimatorController runtime = _initialRuntimeAnimatorController;
        //AnimatorOverrideController over = new AnimatorOverrideController();
        //_overrideController.runtimeAnimatorController = runtime;

        string animName = string.Empty;
        switch (equipmentComponent)
        {
            case EquipmentComponent.LeftArm:
                animName = "PlayerLeftArm0";
                break;
            case EquipmentComponent.RightArm:
                animName = "PlayerRightArm0";
                break;
        }

        for (int i = 0; i < overridedAnimations.Length; i++)
            _overrideController[animName + (i + 1)] = overridedAnimations[i];

        _animator.runtimeAnimatorController = _overrideController;
    }

    #endregion

    #region Ability setup

    public void SetupActiveAbility(ActiveAbility active, EquipmentComponent equipmentComponent)
    {
        if (active == null)
        {
            OverrideActiveAbilityAnimations(_playerMain._initial_activeAbilityAnimations, equipmentComponent);
        }
        else
        {
            OverrideActiveAbilityAnimations(active._abilityAnimations, equipmentComponent);
        }
    }

    public void OverrideActiveAbilityAnimations(AnimationClip overridedAnimation, EquipmentComponent equipmentComponent)
    {
        if (overridedAnimation == null) return;
        //RuntimeAnimatorController runtime = _initialRuntimeAnimatorController;
        //AnimatorOverrideController over = new AnimatorOverrideController();
        //_overrideController.runtimeAnimatorController = runtime;

        string animName = string.Empty;
        switch (equipmentComponent)
        {
            case EquipmentComponent.Head:
                animName = "HeadActiveAbility";
                break;
            case EquipmentComponent.Torso:
                animName = "TorsoActiveAbility";
                break;
            case EquipmentComponent.RightArm:
                animName = "RightArmActiveAbility";
                break;
            case EquipmentComponent.LeftArm:
                animName = "LeftArmActiveAbility";
                break;
            case EquipmentComponent.Legs:
                animName = "LegsActiveAbility";
                break;
        }

        _overrideController[animName] = overridedAnimation;

        _animator.runtimeAnimatorController = _overrideController;
    }

    #endregion
}
