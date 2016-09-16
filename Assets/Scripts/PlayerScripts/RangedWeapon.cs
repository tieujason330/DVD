using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RangedWeapon : BaseWeapon
{
    public Queue<Projectile> _projectiles;
    public float _projectileRange = 10.0f;
    public float _projectileSpeed = 30.0f;

    void Awake()
    {
        base.Awake();
        _projectiles = new Queue<Projectile>(GetComponentsInChildren<Projectile>());
        foreach (var projectile in _projectiles)
        {
            projectile.gameObject.SetActive(false);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Vector3 forward = transform.TransformDirection(Vector3.forward * 2);
        //Debug.DrawRay(transform.position, forward, Color.green);
        if (_myCharacter.IsAttacking() && _myCharacter.IsAiming())
	    {
	        Projectile projectile = TakeProjectile();
	        if (projectile != null)
	        {
	            projectile.Fired();
	        }
	    }
	}

    public void GiveDamage(BaseWorldCharacter attackedCharacter)
    {
        _myCharacter.GiveDamage(10.0f, attackedCharacter);
    }

    public void AddProjectile(Projectile projectile)
    {
        _projectiles.Enqueue(projectile);
    }

    public Projectile TakeProjectile()
    {
        if (_projectiles.Count > 0)
            return _projectiles.Dequeue();
        return null;
    }
}
