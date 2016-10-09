using UnityEngine;
using System.Collections;

public abstract class BaseWorldCharacter : MonoBehaviour
{
    #region initial stats

    public int _initial_attackMaxComboCount;
    public AnimationClip[] _initial_attackAnimations;
    public AnimationClip _initial_activeAbilityAnimations;

    #endregion

    public float _currentHealth;
    public float _initialHealth;
    public Faction _faction;
    public CharacterStatus _status;
    public CharacterAction _action;

    public bool _attackFrame;

    public Vector3 _destinationPosition = Vector3.zero;
    public BaseWorldCharacter _followDetectionCharacter = null;
    public BaseWorldCharacter _attackDetectionCharacter = null;

    public bool IsAttacking { get; set; }

    public void Awake()
    {
        _attackFrame = false;
        _action = CharacterAction.None;
        _currentHealth = _initialHealth;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
	    CheckPlayerStatus();
	}

    public bool IsAiming { get; set; }
    //public abstract bool IsAttacking();
    public abstract bool GiveDamage(float damage, BaseWorldCharacter attackedCharacter);
    public abstract bool TakeDamage(float damage);

    public void AttackAnimationEvent()
    {
        _attackFrame = !_attackFrame;
    }

    public void CheckPlayerStatus()
    {
        _status = _currentHealth <= 0 ? CharacterStatus.Dead : CharacterStatus.Alive;
    }

    public void SetTargetCharacter(BaseWorldCharacter character)
    {
        _followDetectionCharacter = character;
    }

    public abstract void SetDestinationPosition(Vector3 destination);

    public abstract void SetupEquipmentLogic(BaseEquipment equipment, bool equip);
}
