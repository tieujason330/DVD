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
        if (_myCharacter.IsAttacking && !_collisionEntered)
            _collider.enabled = true;

        if (!_myCharacter.IsAttacking)
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
            _myCharacter.GiveDamage(CalculateDamage(), attackedCharacter);
    }

    void OnEnable()
    {
        Equip();
    }

    void OnDisable()
    {
        Unequip();
    }

    private float CalculateDamage()
    {
        return UnityEngine.Random.Range(_minimumDamage, _maximumDamage);
    }

    public override void Equip()
    {
        _myCharacter.SetupEquipmentLogic(this, true);
    }

    public override void Unequip()
    {
        _myCharacter.SetupEquipmentLogic(this, false);
    }
}
