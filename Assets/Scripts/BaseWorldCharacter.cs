using UnityEngine;
using System.Collections;

public abstract class BaseWorldCharacter : MonoBehaviour
{
    public float _currentHealth;
    public float _initialHealth;
    public Faction _faction;
    public CharacterStatus _status;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
	    CheckPlayerStatus();
	}

    public abstract bool IsAttacking();
    public abstract void GiveDamage(float damage, BaseWorldCharacter attackedCharacter);
    public abstract void TakeDamage(float damage);

    public void CheckPlayerStatus()
    {
        if (_currentHealth <= 0)
            _status = CharacterStatus.Dead;
        else
            _status = CharacterStatus.Alive;
    }
}
