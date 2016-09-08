using UnityEngine;
using System.Collections;

public abstract class CharacterRole : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract void PerformAction(string _action);

    public abstract void PerformOwnAction(string _action);

    public abstract void InitializeRole();
}
