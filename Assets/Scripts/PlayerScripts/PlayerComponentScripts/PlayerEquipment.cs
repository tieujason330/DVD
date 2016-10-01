using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerEquipment : MonoBehaviour {

    private Animator _animator;
    private RuntimeAnimatorController _initialRuntimeAnimatorController;
    public PlayerMain _playerMain;

    public BaseEquipment _headEquipment;
    public BaseEquipment _torsoEquipment;
    public BaseEquipment _rightArmEquipment;
    public BaseEquipment _leftArmEquipment;
    public BaseEquipment _legsEquipment;

    void Awake()
    {
        _animator = GetComponent<Animator>();
        //save original runtime to prevent nesting when overriding (Unity issue)
        _initialRuntimeAnimatorController = _animator.runtimeAnimatorController;
        _playerMain = GetComponent<PlayerMain>();
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
            case EquipmentType.MeleeWeapon:
                SetupMeleeWeapon(equipment as MeleeWeapon, equip);
                break;
        }
    }

    #region weapon setup

    void SetupMeleeWeapon(MeleeWeapon meleeWeapon, bool equip)
    {
        if (equip)
        {
            //set up weapon
            if (meleeWeapon._equipmentComponent == EquipmentComponent.LeftArm)
                _playerMain._attackMaxComboCountLeftArm = meleeWeapon._maxAttackComboCount;
            else if (meleeWeapon._equipmentComponent == EquipmentComponent.RightArm)
                _playerMain._attackMaxComboCountRightArm = meleeWeapon._maxAttackComboCount;

            OverrideEquipmentAnimations(meleeWeapon._attackAnimations, meleeWeapon._equipmentComponent);

            //set up weapon's active ability
            SetupActiveAbility(meleeWeapon._activeAbility);
        }
        else
        {
            _playerMain._attackMaxComboCountRightArm = _playerMain._initial_attackMaxComboCount;
            OverrideEquipmentAnimations(_playerMain._initial_attackAnimations, meleeWeapon._equipmentComponent);

            SetupActiveAbility(null);
        }
    }

    public void OverrideEquipmentAnimations(AnimationClip[] overridedAnimations, EquipmentComponent equipmentComponent)
    {
        if (overridedAnimations.Length == 0) return;

        RuntimeAnimatorController runtime = _initialRuntimeAnimatorController;
        AnimatorOverrideController over = new AnimatorOverrideController();
        over.runtimeAnimatorController = runtime;
        
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
            over[animName + (i + 1)] = overridedAnimations[i];

        _animator.runtimeAnimatorController = over;
    }

    #endregion

    public void SetupActiveAbility(ActiveAbility active)
    {
        if (active == null)
        {
            OverrideActiveAbilityAnimations(_playerMain._initial_activeAbilityAnimations);
        }
        else
        {
            OverrideActiveAbilityAnimations(active._abilityAnimations);
        }
    }

    public void OverrideActiveAbilityAnimations(AnimationClip overridedAnimations)
    {
    }

}
