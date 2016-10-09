using UnityEngine;
using System.Collections;

public abstract class BaseEquipment : MonoBehaviour
{
    public BaseWorldCharacter _myCharacter;
    public ActiveAbility _activeAbility;
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

    public abstract void Equip();

    public abstract void Unequip();
}
