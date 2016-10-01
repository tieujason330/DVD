using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InventorySlot : MonoBehaviour {

    public GameObject _slotItem;
    private Sprite _slotSprite;
    public int x;
    public int y;

    public Sprite SlotSprite
    {
        get { return _slotSprite; }
        set { _slotSprite = value; }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetItem(GameObject item)
    {
        Image image = this.GetComponent<Image>();

        if (item == null)
        {
            image.sprite = null;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 100);
        }
        else
        {
            _slotItem = item;
            _slotSprite = item.GetComponent<BaseEquipment>()._itemIcon;
            image.sprite = _slotSprite;
            image.color = new Color(image.color.r, image.color.g, image.color.b, 255);
        }
    }
}
