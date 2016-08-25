using UnityEngine;
using System.Collections;

public abstract class BaseAIUnitGroup : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract void PerformGroupAction(string _action);
}
