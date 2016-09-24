using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

    private GameObject head;
    private bool _open;

    public uint _size;
    public GameObject[] _inventoryList;
    private uint _current;
    private uint _selected;
    private bool _equipping;
    private bool _swapping;


    // This should be assigned the canvas gameobject so that we can edit it directly though the engine
    // inste
    public GameObject _inventoryCanvas; 

	// Use this for initialization
	void Start ()
    {
        _open = false;
        _inventoryList = new GameObject[_size];
	}
	
	// Update is called once per frame
	void Update ()
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
}
