using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseGroup : MonoBehaviour
{
    public List<BaseRole> _units;

    public int _amountPerRow;
    public float _distanceBetweenUnits;

    public void Awake()
    {
        _units = GetComponentsInChildren<BaseRole>().ToList();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update ()
	{
    }

    public bool IsEmpty()
    {
        return _units.Count == 0;
    }

    public abstract void ExecuteCommand(BaseCommand command);

    public virtual void SetGroupFormation()
    {
        //reset in case any children went inactive
        _units = GetComponentsInChildren<BaseRole>().ToList();

        if (_units.Count <= 1)
            return;

        SetTopLeftPosition();

        int multiple = 1;
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;
        for (int i = 1; i < _units.Count; i++)
        {
            BaseRole currentUnit = _units[i];
            if (i < _amountPerRow * multiple)
            {
                Vector3 previousUnitDestination = _units[i - 1]._destination;
                x = previousUnitDestination.x + _distanceBetweenUnits;
                y = previousUnitDestination.y;
                z = previousUnitDestination.z;
            }
            else if (i % _amountPerRow == 0)
            {
                Vector3 previousRowUnitDestination = _units[i - _amountPerRow]._destination;
                x = previousRowUnitDestination.x;
                y = previousRowUnitDestination.y;
                z = previousRowUnitDestination.z - _distanceBetweenUnits;
            
                multiple++;
            }
            currentUnit._destination = new Vector3(x, y, z);

            //currentUnit.transform.position = currentUnit._destination;
            currentUnit.GetComponent<BaseWorldCharacter>().SetDestinationPosition(currentUnit._destination);
        }
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(50, 100, 100, 40), "SetPosition"))
        {
            SetGroupFormation();
        }
    }

    private void SetTopLeftPosition()
    {
        BaseRole topLeftUnit = _units[0];

        float x = transform.position.x - (((_amountPerRow - 1.0f) / 2.0f) * _distanceBetweenUnits);
        float y = transform.position.y;
        float z = transform.position.z;// + (((_formationRowCount - 1.0f) / 2.0f) * _distanceBetweenUnits);
        topLeftUnit._destination = new Vector3(x, y, z);

        //topLeftUnit.transform.position = topLeftUnit._destination;
        topLeftUnit.GetComponent<BaseWorldCharacter>().SetDestinationPosition(topLeftUnit._destination);

    }
}
