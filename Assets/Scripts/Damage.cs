using UnityEngine;
using System.Collections;

//public class Damage : MonoBehaviour {

//	// Use this for initialization
//	void Start () {
	
//	}
	
//	// Update is called once per frame
//	void Update () {
	
//	}
//}

public struct Damage
{
    public float _amount;
    public DamageType _type;

    public Damage(float amount, DamageType type)
    {
        _amount = amount;
        _type = type;
    }
}

public enum DamageType
{
    Normal,
    Stunned,
    Knockback
}
