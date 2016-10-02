using UnityEngine;
using System.Collections;
using System;

public class TestHelmet : BaseEquipment {
    
    private bool _free;
    private PlayerInventory _inventoryOwner;
    private PlayerEquipment _equipmentOwner;

    public Vector3 positionOffSet;
    public Vector3 roationOffset;

    public bool Free
    {
        get { return _free; }
        set { _free = value; }
    }

	// Use this for initialization
	void Start () {
        _free = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void Equip()
    {
        BaseEquipment currenHeadEquipment = _equipmentOwner.HeadEquipment;
        if (currenHeadEquipment != null)
            currenHeadEquipment.Unequip();

        transform.parent = _equipmentOwner.HeadTransform;
        transform.localPosition = positionOffSet;
        transform.localRotation= Quaternion.Euler(roationOffset);
        _equipmentOwner.HeadEquipment = this;
        GetComponent<Collider>().enabled = false;
        gameObject.SetActive(true);
    }

    public override void Unequip()
    {
        gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_free)
        {
            if (collision.collider.tag == "Player")
            {
                _equipmentOwner = collision.transform.GetComponent<PlayerEquipment>();
                _inventoryOwner = collision.transform.GetComponent<PlayerInventory>();
                _inventoryOwner.StoreItem(this.gameObject);
                _free = false;
            }
        }
    }

}
