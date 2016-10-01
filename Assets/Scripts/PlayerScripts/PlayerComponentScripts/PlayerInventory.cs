using UnityEngine;
using UnityEngine.UI;
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
    //public Dictionary<int, GameObject> _slots;
    //public Dictionary<int, GameObject> _items;
    public GameObject[,] _slots;
    private int _currentRow = 0;
    private int _currentColumn = 0;
    private int _selected;
    private bool _equipping;
    private bool _swapping;
    private int _slotCount;
    private GameObject _currentSlot;

    private float _inputHorizontal;
    private float _inputVertical;

   public float _inputRate = 0.25f;
    private float _nextInput = 0.0f;


    // This should be assigned the canvas gameobject so that we can edit it directly though the editor
    public GameObject _inventoryCanvas;

    // Use this for initialization
    void Start()
    {
        _open = false;
        //_slots = new Dictionary<int, GameObject>();
        _slotCount = _maxColumnCount * _maxRowCount;
        _slots = new GameObject[_maxRowCount, _maxColumnCount];

        for (int i = 0; i < _maxRowCount; i++)
        {
            for (int j = 0; j < _maxColumnCount; j++)
            {
                GameObject slot = Instantiate(_slotObject);
                slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(_initialSlotX + (i * _xOffset), _initialSlotY - (j * _yOffset), 0);
                slot.transform.SetParent(_inventoryCanvas.transform);
                _slots[i, j] = slot;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_open)
        {
            UpdateInput();
        }
    }

    void OnGUI()
    {
        if (_open)
        {

        }
    }

    // This fucked up shit makes me think we should do something else instead of list indices
    void UpdateInput()
    {
        _inputHorizontal = Input.GetAxis("InventoryHorizontal");
        _inputVertical = Input.GetAxis("InventoryVertical");

        if (Time.time > _nextInput)
        {
            int previousColumn = _currentColumn;
            int previousRow = _currentRow;

            if (Mathf.Abs(_inputVertical) > 0.5f)
            {
                _currentColumn = (_currentColumn + (int)(_inputVertical / Mathf.Abs(_inputVertical))) % _maxColumnCount;

                if (_currentColumn < 0)
                    _currentColumn = _maxColumnCount - 1;

                print("COLUMN " + _currentColumn);
            }

            if (Mathf.Abs(_inputHorizontal) > 0.5f)
            {
                _currentRow = (_currentRow + (int)(_inputHorizontal / Mathf.Abs(_inputHorizontal))) % _maxRowCount;

                if (_currentRow < 0)
                    _currentRow = _maxRowCount - 1;

                print("ROW" + _currentRow);
            }

            if (previousColumn != _currentColumn || previousRow != _currentRow)
            {
                _nextInput = Time.time + _inputRate;
                _currentSlot.GetComponent<RectTransform>().sizeDelta = new Vector3(75, 75, 0);
                _currentSlot = _slots[_currentRow, _currentColumn];
                _currentSlot.GetComponent<RectTransform>().sizeDelta = new Vector3(90, 90, 0);
            }
        }
    }

    // flip the open variable and return it
    public bool Toggle()
    {
        _open = !_open;
        _inventoryCanvas.SetActive(_open);

        // when the inventory is open the first item in it should be set to 
        // be the currently selected item
        if (_open)
        {
            _currentColumn = 0;
            _currentRow = 0;
            _currentSlot = _slots[0, 0];
            _currentSlot.GetComponent<RectTransform>().sizeDelta = new Vector3(90, 90, 0);
        }
        else
        {
            _currentSlot.GetComponent<RectTransform>().sizeDelta = new Vector3(75, 75, 0);
        }

        return _open;
    }
    public void PlayerUpdate()
    {

    }

    public void StoreItem(GameObject pickup)
    {
        GameObject openSlot = null;

        // Find the first empy slot in the inventory
        for (int i = 0; i < _maxColumnCount; i++)
        {
            for (int j = 0; j < _maxRowCount; j++)
            {

                GameObject currentSlot = _slots[j,i];
                print(currentSlot);
                if (currentSlot.GetComponent<InventorySlot>()._slotItem == null)
                {
                    openSlot = _slots[j, i];
                }

                // once an empty slot is found we can exit both loops
                if (openSlot != null)
                    goto end_of_loop;
            }
        }
        end_of_loop:
            
        if (openSlot == null)
        {
            print("Not enough room in inventory");
        }
        else
        {
            openSlot.GetComponent<InventorySlot>().SetItem(pickup);
            pickup.SetActive(false);
        }
    }
}
