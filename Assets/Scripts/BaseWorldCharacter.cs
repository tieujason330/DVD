using UnityEngine;
using System.Collections;

public abstract class BaseWorldCharacter : MonoBehaviour
{
    public float _currentHealth;
    public float _initialHealth;
    public Faction _faction;
    public CharacterStatus _status;

    public BaseWorldCharacter _followDetectionCharacter = null;
    public BaseWorldCharacter _attackDetectionCharacter = null;
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
        _status = _currentHealth <= 0 ? CharacterStatus.Dead : CharacterStatus.Alive;
    }

    public void SetTargetCharacter(BaseWorldCharacter character)
    {
        _followDetectionCharacter = character;
    }
}
