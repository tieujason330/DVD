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
        MyCharacter.SetupEquipmentLogic(this, true);
    }

    public override void Unequip()
    {
        MyCharacter.SetupEquipmentLogic(this, false);
    }
}
