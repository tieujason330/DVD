using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{

    private GameObject head;
    private bool _open;

    public GameObject _slotObject;
    public int _initialSlotX;
    public int _initialSlotY;
    public int _maxColumnCount;
    public int _maxRowCount;
    public int _xOffset;
    public int _yOffset;

    public uint _size;
    public Dictionary<uint, GameObject> _slots;
    private uint _current;
    private uint _selected;
    private bool _equipping;
    private bool _swapping;


    // This should be assigned the canvas gameobject so that we can edit it directly though the editor
    public GameObject _inventoryCanvas;

    // Use this for initialization
    void Start()
    {
        _open = false;
        _slots = new Dictionary<uint, GameObject>();

        for (int i = 0; i < _maxColumnCount; i++)
        {
            for (int j = 0; j < _maxRowCount; j++)
            {
                GameObject slot = Instantiate(_slotObject);
                slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(_initialSlotX + (j * _xOffset), _initialSlotY - (i * _yOffset), 0);
                slot.transform.SetParent(_inventoryCanvas.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_open)
        {

        }
    }

    void OnGUI()
    {
        if (_open)
        {

        }
    }

    // flip the open variable and return it
    public bool Toggle()
    {
        _open = !_open;
        _inventoryCanvas.SetActive(_open);
        return _open;
    }
    public void PlayerUpdate()
    {

    }
}
