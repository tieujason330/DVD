using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseGroup : MonoBehaviour
{
    public List<BaseRole> _units;

    public void Awake()
    {
        _units = GetComponentsInChildren<BaseRole>().ToList();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool IsEmpty()
    {
        return _units.Count == 0;
    }

    public abstract void ExecuteCommand(BaseCommand command);
}
