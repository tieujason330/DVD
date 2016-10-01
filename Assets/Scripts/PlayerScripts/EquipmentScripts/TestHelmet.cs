using UnityEngine;
using System.Collections;
using System;

public class TestHelmet : BaseEquipment {
    
    private bool _free;

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
        throw new NotImplementedException();
    }

    public override void Unequip()
    {
        throw new NotImplementedException();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_free)
        {
            if (collision.collider.tag == "Player")
            {
                collision.transform.GetComponent<PlayerInventory>().StoreItem(this.gameObject);
                _free = false;
            }
        }
    }

}
