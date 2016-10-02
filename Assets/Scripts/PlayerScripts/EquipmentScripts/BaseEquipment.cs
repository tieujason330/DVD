using UnityEngine;
using System.Collections;

public abstract class BaseEquipment : MonoBehaviour
{
    public ActiveAbility _activeAbility;
    public EquipmentComponent _equipmentComponent;
    public EquipmentType _EquipmentType;
    public Sprite _itemIcon;

    private bool _canEquip;

    public bool CanEquip
    {
        get { return _canEquip; }
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
