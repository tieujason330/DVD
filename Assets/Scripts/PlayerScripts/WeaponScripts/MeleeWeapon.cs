using System;
using UnityEngine;
using System.Collections;

public class MeleeWeapon : BaseWeapon
{
    private BoxCollider _collider;
    private bool _collisionEntered;
    
    public int _maxAttackComboCount;
    public AnimationClip[] _attackAnimations;
    public bool _isEquipped;
    private bool _isPlayerAttacking;
    public float _staminaCost;
    public float _punishMultiplier;
    public float _combatPointsGainMultiplier;

    public DamageType _damageType;

    void Awake()
    {
        base.Awake();
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
        _collisionEntered = false;
    }

	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    void Update()
    {
        IsPlayerAttacking();

        if (_isPlayerAttacking && !_collisionEntered)
            _collider.enabled = true;

        if (!_isPlayerAttacking)
        {
            _collider.enabled = false;
            _collisionEntered = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        _collisionEntered = true;
        _collider.enabled = false;
        BaseWorldCharacter attackedCharacter = collider.gameObject.GetComponent<BaseWorldCharacter>();
        if (attackedCharacter != null)
            MyCharacter.GiveDamage(GetDamage(), attackedCharacter);
    }

    void OnEnable()
    {
        Equip();
    }

    void OnDisable()
    {
        Unequip();
    }

    private void IsPlayerAttacking()
    {
        if (_equipmentComponent == EquipmentComponent.LeftArm)
        {
            _isPlayerAttacking = MyCharacter.IsLeftAttacking;
        }
        else if (_equipmentComponent == EquipmentComponent.RightArm)
        {
            _isPlayerAttacking = MyCharacter.IsRightAttacking;
        }
        else
        {
            _isPlayerAttacking = false;
        }
    }

    private Damage GetDamage()
    {
        float amount = UnityEngine.Random.Range(_minimumDamage, _maximumDamage);
        return new Damage(amount, _damageType);
    }

    public override void Equip()
    {
        MyCharacter.SetupEquipmentLogic(this, true);
    }

    public override void Unequip()
    {
        MyCharacter.SetupEquipmentLogic(this, false);
    }
}
