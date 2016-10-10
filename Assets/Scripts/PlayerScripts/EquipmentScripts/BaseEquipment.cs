using UnityEngine;
using System.Collections;

public abstract class BaseEquipment : MonoBehaviour
{
    private BaseWorldCharacter _myCharacter;
    private ActiveAbility _activeAbility;
    public EquipmentComponent _equipmentComponent;
    public EquipmentType _EquipmentType;
    public Sprite _itemIcon;
    public float _activeAbilityCost;

    public bool _equipped;

    private bool _canEquip;

    public bool CanEquip
    {
        get { return _canEquip; }
    }

    public void Awake()
    {
        _myCharacter = GetComponentInParent<BaseWorldCharacter>();

        _equipped = false;
        InitializeActiveAbility();
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void InitializeActiveAbility()
    {
        _activeAbility = GetComponentInChildren<ActiveAbility>();
        if (_activeAbility != null)
            _activeAbility._activeCost = _activeAbilityCost;
    }

    public ActiveAbility ActiveAbility { get { return _activeAbility; } }

    public BaseWorldCharacter MyCharacter { get { return _myCharacter; } }

    public abstract void Equip();

    public abstract void Unequip();
}
