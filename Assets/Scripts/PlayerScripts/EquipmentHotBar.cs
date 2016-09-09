using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentHotBar : MonoBehaviour
{
    public Button _headSlot;
    public Button _leftHandSlot;
    public Button _rightHandSlot;
    public Button _torsoSlot;
    public Button _feetSlot;

    private bool _hotbar1Clicked;
    private bool _hotbar2Clicked;
    private bool _hotbar3Clicked;
    private bool _hotbar4Clicked;
    private bool _hotbar5Clicked;
    private PointerEventData _pointer;

    void Awake()
    {
        Button[] buttons = GetComponentsInChildren<Button>();
        _headSlot = buttons.FirstOrDefault(x => x.name == "Head");
        _leftHandSlot = buttons.FirstOrDefault(x => x.name == "LeftHand");
        _rightHandSlot = buttons.FirstOrDefault(x => x.name == "RightHand");
        _torsoSlot = buttons.FirstOrDefault(x => x.name == "Torso");
        _feetSlot = buttons.FirstOrDefault(x => x.name == "Feet");
    }

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update ()
	{
        _pointer = new PointerEventData(EventSystem.current);

	    _hotbar1Clicked = Input.GetButtonDown("Hotbar1");
        _hotbar2Clicked = Input.GetButtonDown("Hotbar2");
        _hotbar3Clicked = Input.GetButtonDown("Hotbar3");
        _hotbar4Clicked = Input.GetButtonDown("Hotbar4");
        _hotbar5Clicked = Input.GetButtonDown("Hotbar5");

	    ClickButtons();
	}

    private void ClickButtons()
    {
        if (_hotbar1Clicked)
        {
            _headSlot.interactable = !_headSlot.interactable;
            //hover
            //ExecuteEvents.Execute(_equipmentList[0].gameObject, _pointer, ExecuteEvents.pointerEnterHandler);
           //click
            //ExecuteEvents.Execute(_equipmentList[0].gameObject, _pointer, ExecuteEvents.pointerDownHandler);
        }
        else if (_hotbar2Clicked)
            _leftHandSlot.interactable = !_leftHandSlot.interactable;
        else if (_hotbar3Clicked)
            _rightHandSlot.interactable = !_rightHandSlot.interactable;
        else if (_hotbar4Clicked)
            _torsoSlot.interactable = !_torsoSlot.interactable;
        else if (_hotbar5Clicked)
            _feetSlot.interactable = !_feetSlot.interactable;
    }
}
