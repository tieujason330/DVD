using System;
using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
    BaseWorldCharacter _myCharacter;

    public float _minimumDamage = 0.0f;
    public float _maximumDamage = 0.0f;

    private BoxCollider _collider;
    private bool _collisionEntered;
    //

    void Awake()
    {
        _myCharacter = GetComponentInParent<BaseWorldCharacter>();
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = false;
        _collisionEntered = false;
    }

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update()
    {
        if (_myCharacter.IsAttacking() && !_collisionEntered)
            _collider.enabled = true;

        if (!_myCharacter.IsAttacking())
        {
            _collider.enabled = false;
            _collisionEntered = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        _collisionEntered = true;
        _collider.enabled = false;
        BaseWorldCharacter attackedCharacter = collider.gameObject.GetComponent<BaseWorldCharacter>();
        if (attackedCharacter != null)
            _myCharacter.GiveDamage(CalculateDamage(), attackedCharacter);
    }

    private float CalculateDamage()
    {
        return UnityEngine.Random.Range(_minimumDamage, _maximumDamage);
    }
}
