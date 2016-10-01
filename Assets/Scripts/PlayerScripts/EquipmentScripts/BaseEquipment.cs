using UnityEngine;
using System.Collections;

public abstract class BaseEquipment : MonoBehaviour
{
    protected ActiveAbility _activeAbility;
    public EquipmentComponent _equipmentComponent;
    public EquipmentType _EquipmentType;
    public Sprite _itemIcon;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract void Equip();

    public abstract void Unequip();
}
