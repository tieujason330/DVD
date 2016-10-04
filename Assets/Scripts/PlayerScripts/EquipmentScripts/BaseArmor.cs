using UnityEngine;
using System.Collections;
using System;

public class BaseArmor : BaseEquipment {

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

    void OnEnable()
    {
        Equip();
    }

    void OnDisable()
    {
        Unequip();
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
