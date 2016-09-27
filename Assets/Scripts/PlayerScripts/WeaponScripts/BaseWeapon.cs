using UnityEngine;
using System.Collections;

public abstract class BaseWeapon : BaseEquipment {

    public BaseWorldCharacter _myCharacter;

    public float _minimumDamage = 0.0f;
    public float _maximumDamage = 0.0f;

    public void Awake()
    {
        _myCharacter = GetComponentInParent<BaseWorldCharacter>();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
