using UnityEngine;
using System.Collections;

public abstract class BaseWorldCharacter : MonoBehaviour
{
    public float _currentHealth;
    public float _initialHealth;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public abstract bool IsAttacking();
    public abstract void GiveDamage(float damage, BaseWorldCharacter attackedCharacter);

    public abstract void TakeDamage(float damage);
}
