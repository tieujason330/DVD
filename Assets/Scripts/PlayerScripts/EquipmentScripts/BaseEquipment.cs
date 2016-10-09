using UnityEngine;
using System.Collections;

public abstract class BaseEquipment : MonoBehaviour
{
    private BaseWorldCharacter _myCharacter;
    private ActiveAbility _activeAbility;
    public EquipmentComponent _equipmentComponent;
    public EquipmentType _EquipmentType;
    public Sprite _itemIcon;

    public bool _equipped;

    private bool _canEquip;

    public bool CanEquip
    {
        get { return _canEquip; }
    }

    public void Awake()
    {
        _myCharacter = GetComponentInParent<BaseWorldCharacter>();
        _activeAbility = GetComponentInChildren<ActiveAbility>();
        _equipped = false;
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public ActiveAbility ActiveAbility { get { return _activeAbility; } }

    public BaseWorldCharacter MyCharacter { get { return _myCharacter; } }

    public abstract void Equip();

    public abstract void Unequip();
}
